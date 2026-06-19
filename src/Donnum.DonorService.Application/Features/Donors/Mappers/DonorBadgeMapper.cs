using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Mappers;

public static class DonorBadgeMapper
{
    public static DonorBadgeDto MapToDto(DonorBadge donorBadge) => new DonorBadgeDto(
        donorBadge.Badge.Id,
        donorBadge.Badge.Name,
        donorBadge.Badge.BadgeType,
        donorBadge.Badge.BadgePoints,
        donorBadge.AssignedAt
    );
}
