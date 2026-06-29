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
        var donorsWithAttendance = await _donorRepository.GetDonorsWithAttendanceByRequestIdAsync(request.RequestId, cancellationToken);
        
        return donorsWithAttendance.Select(x => DonorMapper.ToDto(x.Donor, x.Attended)).ToList();
    }
}
