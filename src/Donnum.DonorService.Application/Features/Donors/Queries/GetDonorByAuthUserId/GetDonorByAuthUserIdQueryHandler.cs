using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorByAuthUserId;

public sealed class GetDonorByAuthUserIdQueryHandler : IRequestHandler<GetDonorByAuthUserIdQuery, Guid>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorByAuthUserIdQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<Guid> Handle(GetDonorByAuthUserIdQuery request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetByAuthUserIdAsync(request.AuthUserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.AuthUserId);

        return donor.Id;
    }
}
