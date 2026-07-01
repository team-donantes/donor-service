using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;
using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donations.Mappers;
using MediatR;
using Donnum.DonorService.Application.Features.Donors.Services;

namespace Donnum.DonorService.Application.Features.Donors.Commands.RegisterAttendance;

public sealed class RegisterAttendanceCommandHandler : IRequestHandler<RegisterAttendanceCommand>
{
    private readonly IDonorRepository _donorRepository;
    private readonly IDonationRepository _donationRepository;
    private readonly IIntegrationEventOutbox _outbox;
    private readonly Application.Features.Donors.Services.IReliabilityCalculator _reliabilityCalculator;
    private readonly IEvaluateAndAssignBadgesService _evaluateAndAssignBadgesService;

    public RegisterAttendanceCommandHandler(
        IDonorRepository donorRepository,
        IDonationRepository donationRepository,
        IIntegrationEventOutbox outbox,
        Application.Features.Donors.Services.IReliabilityCalculator reliabilityCalculator,
        IEvaluateAndAssignBadgesService evaluateAndAssignBadgesService)
    {
        _donorRepository = donorRepository;
        _donationRepository = donationRepository;
        _outbox = outbox;
        _reliabilityCalculator = reliabilityCalculator;
        _evaluateAndAssignBadgesService = evaluateAndAssignBadgesService;
    }

    public async Task Handle(RegisterAttendanceCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetWithReliabilityScoreByIdAsync(request.DonorId, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

        await EnsureReliabilityScoreExistsAsync(donor, cancellationToken);

        var existingDonations = await _donationRepository.GetByDonorIdAsync(donor.Id, cancellationToken);

        if (request.Attended && existingDonations.Any(d => d.DonationRequestId == request.DonationRequestId))
        {
            return;
        }

        var newScore = _reliabilityCalculator.CalculateNewScore(
            donor.ReliabilityScore!.Score,
            donor.Gender,
            existingDonations,
            request.DonationDate,
            request.Attended
        );

        donor.UpdateReliabilityScore(newScore);

        _donorRepository.Update(donor);

        if (request.Attended)
        {
            var donation = DonationMapper.ToEntity(request);

            await _donationRepository.AddAsync(donation, cancellationToken);
            donor.Points += 50;
        }

        var participation = await _donationRepository.GetParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
        if (participation != null)
        {
            participation.Status = request.Attended ? Domain.Enums.ParticipationStatus.Attended : Domain.Enums.ParticipationStatus.Missed;
            participation.UpdatedAt = DateTime.UtcNow;
        }


        if (request.Attended)
        {
            var eventId = Guid.NewGuid();
            var bloodType = $"{donor.BloodGroup}{(donor.RhFactor == "+" || donor.RhFactor.StartsWith("P", StringComparison.OrdinalIgnoreCase) ? "+" : "-")}";
            await _outbox.EnqueueAsync(
                Exchanges.Donor,
                DonnumEventTopics.DonorDonationRegistered,
                new
                {
                    EventId = eventId,
                    OccurredAt = DateTimeOffset.UtcNow,
                    DonorId = donor.Id,
                    RequestId = request.DonationRequestId,
                    MedicalCenterId = request.MedicalCenterId,
                    DonationDate = request.DonationDate,
                    BloodType = bloodType
                },
                request.DonationRequestId.ToString("D"),
                eventId,
                cancellationToken);

            if (await _donationRepository.IsCampaignRequestAsync(request.DonationRequestId, cancellationToken))
            {
                var attendanceEventId = Guid.NewGuid();
                await _outbox.EnqueueAsync(
                    Exchanges.Request,
                    DonnumEventTopics.RequestCampaignAttendanceRegistered,
                    new
                    {
                        EventId = attendanceEventId,
                        OccurredAt = DateTimeOffset.UtcNow,
                        CampaignId = request.DonationRequestId,
                        DonorId = donor.Id,
                        RegisteredAt = DateTimeOffset.UtcNow
                    },
                    request.DonationRequestId.ToString("D"),
                    attendanceEventId,
                    cancellationToken);
            }
        }

        await _donorRepository.SaveChangesAsync(cancellationToken);
        
        if (request.Attended)
        {
            await _evaluateAndAssignBadgesService.ExecuteAsync(donor.Id, cancellationToken);
        }
    }

    private async Task EnsureReliabilityScoreExistsAsync(Donor donor, CancellationToken cancellationToken)
    {
        if (donor.ReliabilityScore == null)
        {
            donor.ReliabilityScore = new ReliabilityScore
            {
                DonorId = donor.Id,
                Score = 100,
                LastCalculatedAt = DateTime.UtcNow
            };
            await _donorRepository.AddReliabilityScoreAsync(donor.ReliabilityScore, cancellationToken);
        }
    }
}
