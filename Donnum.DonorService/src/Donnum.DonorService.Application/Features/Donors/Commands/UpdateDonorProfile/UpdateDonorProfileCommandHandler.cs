using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public sealed class UpdateDonorProfileCommandHandler : IRequestHandler<UpdateDonorProfileCommand>
{
    private readonly IDonorRepository _donorRepository;

    public UpdateDonorProfileCommandHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task Handle(UpdateDonorProfileCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.Id);

        request.ApplyTo(donor);

        _donorRepository.Update(donor);
        await _donorRepository.SaveChangesAsync(cancellationToken);
    }
}
