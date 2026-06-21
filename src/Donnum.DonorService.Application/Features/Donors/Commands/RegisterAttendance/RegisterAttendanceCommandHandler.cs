using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.RegisterAttendance;

public sealed class RegisterAttendanceCommandHandler : IRequestHandler<RegisterAttendanceCommand>
{
    private readonly IDonorRepository _donorRepository;
    private readonly IDonationRepository _donationRepository;
    private readonly IEventBus _eventBus;
    private readonly Application.Features.Donors.Services.IReliabilityCalculator _reliabilityCalculator;

    public RegisterAttendanceCommandHandler(
        IDonorRepository donorRepository,
        IDonationRepository donationRepository,
        IEventBus eventBus,
        Application.Features.Donors.Services.IReliabilityCalculator reliabilityCalculator)
    {
        _donorRepository = donorRepository;
        _donationRepository = donationRepository;
        _eventBus = eventBus;
        _reliabilityCalculator = reliabilityCalculator;
    }

    public async Task Handle(RegisterAttendanceCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetWithReliabilityScoreByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

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

        var existingDonations = await _donationRepository.GetByDonorIdAsync(donor.Id, cancellationToken);

        if (request.Attended && existingDonations.Any(d => d.DonationRequestId == request.DonationRequestId))
        {
            // Already registered for this request
            return;
        }

        donor.ReliabilityScore.Score = _reliabilityCalculator.CalculateNewScore(
            donor.ReliabilityScore.Score,
            donor,
            existingDonations,
            request.DonationDate,
            request.Attended
        );
        donor.ReliabilityScore.LastCalculatedAt = DateTime.UtcNow;

        _donorRepository.Update(donor);

        if (request.Attended)
        {
            var donation = new Donation
            {
                DonorId = donor.Id,
                DonationRequestId = request.DonationRequestId,
                MedicalCenterId = request.MedicalCenterId,
                DonationDate = request.DonationDate,
                CreatedAt = DateTime.UtcNow
            };

            await _donationRepository.AddAsync(donation, cancellationToken);
        }

        await _donorRepository.SaveChangesAsync(cancellationToken);

        if (request.Attended)
        {
            var eventPayload = new
            {
                DonorId = donor.Id,
                DonationRequestId = request.DonationRequestId,
                MedicalCenterId = request.MedicalCenterId,
                DonationDate = request.DonationDate,
                RegisteredAt = DateTime.UtcNow
            };

            var messageEnvelope = new MessageEnvelope
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                Topic = "donor.donation.registered",
                Payload = JsonSerializer.Serialize(eventPayload)
            };

            await _eventBus.PublishAsync(messageEnvelope, cancellationToken);
        }
    }
}
