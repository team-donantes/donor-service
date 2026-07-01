using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CreateParticipation;

public sealed class CreateParticipationCommandHandler : IRequestHandler<CreateParticipationCommand>
{
    private readonly IDonationRepository _donationRepository;
    private readonly IIntegrationEventOutbox _outbox;

    public CreateParticipationCommandHandler(IDonationRepository donationRepository, IIntegrationEventOutbox outbox)
    {
        _donationRepository = donationRepository;
        _outbox = outbox;
    }

    public async Task Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
    {
        var existingParticipation = await _donationRepository.GetParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
        if (existingParticipation != null)
        {
            existingParticipation.Status = request.Status;
            existingParticipation.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var participation = new DonationRequestParticipation
            {
                DonorId = request.DonorId,
                DonationRequestId = request.DonationRequestId,
                Status = request.Status,
                RegisteredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _donationRepository.AddParticipationAsync(participation, cancellationToken);
        }

        var eventId = Guid.NewGuid();
        var topic = request.Status == Domain.Enums.ParticipationStatus.Confirmed
            ? DonnumEventTopics.DonorAvailabilityConfirmed
            : DonnumEventTopics.DonorAvailabilityDeclined;
        await _outbox.EnqueueAsync(
            Exchanges.Donor,
            topic,
            new
            {
                EventId = eventId,
                OccurredAt = DateTimeOffset.UtcNow,
                request.DonorId,
                RequestId = request.DonationRequestId,
                RespondedAt = DateTimeOffset.UtcNow
            },
            request.DonationRequestId.ToString("D"),
            eventId,
            cancellationToken);
        await _donationRepository.SaveChangesAsync(cancellationToken);
    }
}
