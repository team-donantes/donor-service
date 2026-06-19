using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public class GetDonationHistoryQueryHandler(IDonationRepository donationRepository, IDonorRepository donorRepository)
    : IRequestHandler<GetDonationHistoryQuery, DonorDonationHistoryDto?>
{
    public async Task<DonorDonationHistoryDto?> Handle(GetDonationHistoryQuery request, CancellationToken cancellationToken)
    {
        if (!await donorRepository.ExistsAsync(request.DonorId, cancellationToken))
        {
            return null;
        }

        var donations = await donationRepository.GetByDonorIdAsync(request.DonorId, cancellationToken);

        return DonationMapper.MapToDto(request.DonorId, donations);
    }
}
