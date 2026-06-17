using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed class CreateDonorProfileCommandHandler : IRequestHandler<CreateDonorProfileCommand, Guid>
{
    private readonly IDonorRepository _donorRepository;

    public CreateDonorProfileCommandHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<Guid> Handle(CreateDonorProfileCommand request, CancellationToken cancellationToken)
    {
        if (await _donorRepository.ExistsByAuthUserIdAsync(request.AuthUserId, cancellationToken))
            throw new DomainException($"Ya existe un perfil de donante para el usuario '{request.AuthUserId}'.");

        var donor = request.ToEntity();

        await _donorRepository.AddAsync(donor, cancellationToken);
        await _donorRepository.SaveChangesAsync(cancellationToken);

        return donor.Id;
    }
}
