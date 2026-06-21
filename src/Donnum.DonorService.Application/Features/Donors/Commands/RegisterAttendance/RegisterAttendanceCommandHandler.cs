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

    public RegisterAttendanceCommandHandler(
        IDonorRepository donorRepository,
        IDonationRepository donationRepository,
        IEventBus eventBus)
    {
        _donorRepository = donorRepository;
        _donationRepository = donationRepository;
        _eventBus = eventBus;
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

        if (!request.Attended)
        {
            // Deduction for absence
            donor.ReliabilityScore.Score = Math.Max(0, donor.ReliabilityScore.Score - 20);
            donor.ReliabilityScore.LastCalculatedAt = DateTime.UtcNow;
            
            await _donorRepository.SaveChangesAsync(cancellationToken);
            return;
        }

        // Has attended
        var existingDonations = await _donationRepository.GetByDonorIdAsync(donor.Id, cancellationToken);
        
        if (existingDonations.Any(d => d.DonationRequestId == request.DonationRequestId))
        {
            // Already registered for this request
            return;
        }

        var oneYearAgo = DateTime.UtcNow.AddDays(-365);
        var recentDonationsCount = existingDonations.Count(d => d.DonationDate >= oneYearAgo);

        bool exceededLimits = donor.Gender == Gender.Male ? recentDonationsCount >= 4 : recentDonationsCount >= 3;

        var lastDonation = existingDonations.OrderByDescending(d => d.DonationDate).FirstOrDefault();
        bool violatesInterval = lastDonation != null && (request.DonationDate - lastDonation.DonationDate).TotalDays < 56;

        if (exceededLimits || violatesInterval)
        {
            donor.ReliabilityScore.Score = Math.Max(0, donor.ReliabilityScore.Score - 30);
        }
        else
        {
            donor.ReliabilityScore.Score = Math.Min(100, donor.ReliabilityScore.Score + 10);
        }
        
        donor.ReliabilityScore.LastCalculatedAt = DateTime.UtcNow;

        var donation = new Donation
        {
            DonorId = donor.Id,
            DonationRequestId = request.DonationRequestId,
            MedicalCenterId = request.MedicalCenterId,
            DonationDate = request.DonationDate,
            CreatedAt = DateTime.UtcNow
        };

        await _donationRepository.AddAsync(donation, cancellationToken);
        await _donorRepository.SaveChangesAsync(cancellationToken);

        // Publish event
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
