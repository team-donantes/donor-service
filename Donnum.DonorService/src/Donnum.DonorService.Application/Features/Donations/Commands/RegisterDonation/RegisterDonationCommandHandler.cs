using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donors.Commands.AddPoints;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Commands.RegisterDonation;

public sealed class RegisterDonationCommandHandler(
    IDonorRepository donorRepository,
    IDonationRepository donationRepository,
    IMediator mediator) : IRequestHandler<RegisterDonationCommand>
{
    private const int PointsPerDonation = 25;

    public async Task Handle(RegisterDonationCommand request, CancellationToken cancellationToken)
    {
        var donor = await donorRepository.GetByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

        var donation = new Donation
        {
            Id = Guid.NewGuid(),
            DonorId = request.DonorId,
            DonationRequestId = request.DonationRequestId,
            MedicalCenterId = request.MedicalCenterId,
            DonationDate = request.DonationDate,
            CreatedAt = request.CreatedAt
        };

        await donationRepository.AddAsync(donation, cancellationToken);
        await donationRepository.SaveChangesAsync(cancellationToken);

        await mediator.Send(new AddPointsCommand(request.DonorId, PointsPerDonation), cancellationToken);
    }
}
