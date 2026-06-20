using System.Threading;
using System.Threading.Tasks;
using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddDonorPoints;

public sealed class AddDonorPointsCommandHandler(IDonorRepository donorRepository)
    : IRequestHandler<AddDonorPointsCommand>
{
    public async Task Handle(AddDonorPointsCommand request, CancellationToken cancellationToken)
    {
        var donor = await donorRepository.GetByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

        donor.Points += request.Points;

        donorRepository.Update(donor);
        await donorRepository.SaveChangesAsync(cancellationToken);
    }
}
