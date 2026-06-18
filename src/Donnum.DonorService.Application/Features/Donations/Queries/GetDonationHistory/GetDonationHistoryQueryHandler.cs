using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public class GetDonationHistoryQueryHandler(
    IDonorRepository donorRepository,
    IDonationRepository donationRepository)
    : IRequestHandler<GetDonationHistoryQuery, DonorDonationHistoryDto?>
{
    public async Task<DonorDonationHistoryDto?> Handle(GetDonationHistoryQuery request, CancellationToken cancellationToken)
    {
        var donorExists = await donorRepository.ExistsAsync(request.DonorId, cancellationToken);
        if (!donorExists)
        {
            return null;
        }

        var donations = await donationRepository.GetByDonorIdAsync(request.DonorId, cancellationToken);

        return DonationMapper.MapToDto(request.DonorId, donations);
    }
}
