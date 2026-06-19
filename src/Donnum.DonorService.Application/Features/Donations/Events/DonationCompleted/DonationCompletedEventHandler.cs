using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donations.Mappers;
using Donnum.DonorService.Application.Features.Donors.Commands.AddPoints;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;

public sealed class DonationCompletedEventHandler(
    IDonorRepository donorRepository,
    IDonationRepository donationRepository,
    IMediator mediator) : IRequestHandler<DonationCompletedEvent>
{
    private const int PointsPerDonation = 25;

    public async Task Handle(DonationCompletedEvent request, CancellationToken cancellationToken)
    {
        if (!await donorRepository.ExistsAsync(request.DonorId, cancellationToken))
        {
            throw new NotFoundException(nameof(Donor), request.DonorId);
        }

        var donation = DonationMapper.ToEntity(request);

        await donationRepository.AddAsync(donation, cancellationToken);
        await donationRepository.SaveChangesAsync(cancellationToken);

        await mediator.Send(new AddPointsCommand(request.DonorId, PointsPerDonation), cancellationToken);
    }
}
