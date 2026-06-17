using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donors.Dtos;

public sealed record DonorBadgeDto(
    Guid Id,
    string Name,
    BadgeType BadgeType,
    int BadgePoints,
    DateTime AssignedAt
)
{
    public static DonorBadgeDto FromDonorBadge(DonorBadge donorBadge)
        => new(
            Id: donorBadge.Badge.Id,
            Name: donorBadge.Badge.Name,
            BadgeType: donorBadge.Badge.BadgeType,
            BadgePoints: donorBadge.Badge.BadgePoints,
            AssignedAt: donorBadge.AssignedAt
        );
}
