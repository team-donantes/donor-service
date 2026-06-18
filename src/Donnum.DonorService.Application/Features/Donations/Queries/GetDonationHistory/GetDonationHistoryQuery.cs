using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public record GetDonationHistoryQuery(Guid DonorId) : IRequest<DonorDonationHistoryDto?>;
