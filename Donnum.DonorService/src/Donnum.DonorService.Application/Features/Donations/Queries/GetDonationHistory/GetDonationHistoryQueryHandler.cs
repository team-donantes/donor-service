using Donnum.DonorService.Domain.Repositories;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public class GetDonationHistoryQueryHandler(IDonationRepository donationRepository)
    : IRequestHandler<GetDonationHistoryQuery, DonorDonationHistoryDto?>
{
    public async Task<DonorDonationHistoryDto?> Handle(GetDonationHistoryQuery request, CancellationToken cancellationToken)
    {
        var donations = await donationRepository.GetByDonorIdAsync(request.DonorId, cancellationToken);

        if (donations is null)
        {
            return null;
        }

        return DonationMapper.MapToDto(request.DonorId, donations);
    }
}
