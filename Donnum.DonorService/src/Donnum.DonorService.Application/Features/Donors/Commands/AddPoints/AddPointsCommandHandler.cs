using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddPoints;

public sealed class AddPointsCommandHandler(IDonorRepository donorRepository) : IRequestHandler<AddPointsCommand>
{
    public async Task Handle(AddPointsCommand request, CancellationToken cancellationToken)
    {
        var donor = await donorRepository.GetByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), request.DonorId);

        donor.Points += request.Points;
        donor.UpdatedAt = DateTime.UtcNow;

        donorRepository.Update(donor);
        await donorRepository.SaveChangesAsync(cancellationToken);
    }
}
