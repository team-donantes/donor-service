using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CreateParticipation;

public sealed class CreateParticipationCommandHandler : IRequestHandler<CreateParticipationCommand>
{
    private readonly IDonationRepository _donationRepository;

    public CreateParticipationCommandHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    public async Task Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
    {
        var existingParticipation = await _donationRepository.GetParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
        if (existingParticipation != null)
        {
            throw new InvalidOperationException("Donor is already participating in this request.");
        }

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
        await _donationRepository.SaveChangesAsync(cancellationToken);
    }
}
