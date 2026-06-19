using System.Linq.Expressions;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Mappers;

public static class DonorBadgeMapper
{
    public static Expression<Func<DonorBadge, DonorBadgeDto>> ToDto => db => new DonorBadgeDto(
        db.Badge.Id,
        db.Badge.Name,
        db.Badge.BadgeType,
        db.Badge.BadgePoints,
        db.AssignedAt
    );
}
