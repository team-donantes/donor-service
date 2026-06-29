using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorsByRequest;

public sealed class GetDonorsByRequestQueryHandler : IRequestHandler<GetDonorsByRequestQuery, IReadOnlyList<DonorProfileDto>>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorsByRequestQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<IReadOnlyList<DonorProfileDto>> Handle(GetDonorsByRequestQuery request, CancellationToken cancellationToken)
    {
        var participations = await _donorRepository.GetDonorsByRequestIdAsync(request.RequestId, cancellationToken);
        
        return participations.Select(p => {
            var dto = DonorMapper.ToDto(p.Donor);
            bool? attendedStatus = p.Status switch
            {
                Domain.Enums.ParticipationStatus.Attended => true,
                Domain.Enums.ParticipationStatus.Missed => false,
                _ => null
            };
            return dto with { Attended = attendedStatus };
        }).ToList();
    }
}
