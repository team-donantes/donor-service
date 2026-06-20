using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donations.Mappers;
using Donnum.DonorService.Application.Features.Donors.Services;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;

public sealed class DonationCompletedEventHandler(
    IDonorRepository donorRepository,
    IDonationRepository donationRepository,
    IEvaluateAndAssignBadgesService evaluateAndAssignBadgesService) : IRequestHandler<DonationCompletedEvent>
{
    public async Task Handle(DonationCompletedEvent request, CancellationToken cancellationToken)
    {
        var donor = await donorRepository.GetByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

        var donation = DonationMapper.ToEntity(request);

        await donationRepository.AddAsync(donation, cancellationToken);
        await donationRepository.SaveChangesAsync(cancellationToken);

        donor.Points += 100;
        donorRepository.Update(donor);
        await donorRepository.SaveChangesAsync(cancellationToken);

        await evaluateAndAssignBadgesService.ExecuteAsync(donor.Id, cancellationToken);
    }
}
