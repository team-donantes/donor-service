using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorReliability;

public sealed class GetDonorReliabilityQueryHandler : IRequestHandler<GetDonorReliabilityQuery, DonorReliabilityDto>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorReliabilityQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<DonorReliabilityDto> Handle(GetDonorReliabilityQuery request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetWithReliabilityScoreByIdAsync(request.DonorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.DonorId);

        var score = donor.ReliabilityScore?.Score ?? 100;
        var lastCalculatedAt = donor.ReliabilityScore?.LastCalculatedAt ?? DateTime.UtcNow;

        return new DonorReliabilityDto(
            DonorId: donor.Id,
            Score: score,
            LastCalculatedAt: lastCalculatedAt
        );
    }
}
