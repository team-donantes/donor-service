using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorBadges;

public sealed class GetDonorBadgesQueryHandler : IRequestHandler<GetDonorBadgesQuery, IReadOnlyList<DonorBadgeDto>>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorBadgesQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<IReadOnlyList<DonorBadgeDto>> Handle(GetDonorBadgesQuery request, CancellationToken cancellationToken)
    {
        if (!await _donorRepository.ExistsAsync(request.DonorId, cancellationToken))
            throw new NotFoundException(nameof(Domain.Entities.Donor), request.DonorId);

        var badges = await _donorRepository.GetBadgesByDonorIdAsync(
            request.DonorId, 
            Donnum.DonorService.Application.Features.Donors.Mappers.DonorBadgeMapper.ToDto, 
            cancellationToken);

        return badges;
    }
}
