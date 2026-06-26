using Donnum.DonorService.Domain.Enums;
using Donnum.DonorService.Domain.Repositories;
using MediatR;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CancelParticipation;

public sealed class CancelParticipationCommandHandler : IRequestHandler<CancelParticipationCommand>
{
    private readonly IDonationRepository _donationRepository;
    private readonly IIntegrationEventOutbox _outbox;

    public CancelParticipationCommandHandler(IDonationRepository donationRepository, IIntegrationEventOutbox outbox)
    {
        _donationRepository = donationRepository;
        _outbox = outbox;
    }

    public async Task Handle(CancelParticipationCommand request, CancellationToken cancellationToken)
    {
        var participation = await _donationRepository.GetParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
        if (participation == null)
        {
            throw new KeyNotFoundException("Participation not found.");
        }

        participation.Status = ParticipationStatus.Cancelled;
        participation.UpdatedAt = DateTime.UtcNow;

        var eventId = Guid.NewGuid();
        await _outbox.EnqueueAsync(
            Exchanges.Donor,
            DonnumEventTopics.DonorAvailabilityDeclined,
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
