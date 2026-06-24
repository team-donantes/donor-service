using Donnum.DonorService.Domain.Enums;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CancelParticipation;

public sealed class CancelParticipationCommandHandler : IRequestHandler<CancelParticipationCommand>
{
    private readonly IDonationRepository _donationRepository;

    public CancelParticipationCommandHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
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

        await _donationRepository.SaveChangesAsync(cancellationToken);
    }
}
