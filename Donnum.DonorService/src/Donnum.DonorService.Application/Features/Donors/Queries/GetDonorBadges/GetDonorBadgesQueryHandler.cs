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
        var donor = await _donorRepository.GetByIdWithBadgesAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.DonorId);

        return donor.DonorBadges
            .Select(DonorBadgeDto.FromDonorBadge)
            .ToList()
            .AsReadOnly();
    }
}
