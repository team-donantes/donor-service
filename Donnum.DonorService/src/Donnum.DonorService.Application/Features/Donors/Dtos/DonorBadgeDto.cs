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
}
