using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;

public sealed class DonationCompletedEventHandler(
    IDonorRepository donorRepository,
    IDonationRepository donationRepository) : IRequestHandler<DonationCompletedEvent>
{
    public async Task Handle(DonationCompletedEvent request, CancellationToken cancellationToken)
    {
        if (!await donorRepository.ExistsAsync(request.DonorId, cancellationToken))
        {
            throw new NotFoundException(nameof(Donor), request.DonorId);
        }

        var donation = new Donation
        {
            DonorId = request.DonorId,
            DonationRequestId = request.DonationRequestId,
            MedicalCenterId = request.MedicalCenterId,
            DonationDate = request.DonationDate,
            CreatedAt = request.CreatedAt
        };

        await donationRepository.AddAsync(donation, cancellationToken);
        await donationRepository.SaveChangesAsync(cancellationToken);
    }
}
