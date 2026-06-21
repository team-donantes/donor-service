using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorProfile;

public sealed class GetDonorProfileQueryHandler : IRequestHandler<GetDonorProfileQuery, DonorProfileDto>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorProfileQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<DonorProfileDto> Handle(GetDonorProfileQuery request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetWithReliabilityScoreByIdAsync(request.Id, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.Id);

        return DonorMapper.ToDto(donor);
    }
}
