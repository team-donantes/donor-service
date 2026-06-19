using Donnum.DonorService.Application.Features.Donors.Dtos;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorBadges;

public sealed record GetDonorBadgesQuery(Guid DonorId) : IRequest<IReadOnlyList<DonorBadgeDto>>;
