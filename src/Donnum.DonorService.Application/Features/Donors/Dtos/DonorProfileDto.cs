using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Dtos;

public sealed record DonorProfileDto(
    Guid Id,
    Guid AuthUserId,
    string BloodGroup,
    string RhFactor,
    Donnum.DonorService.Domain.Enums.Gender Gender,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude,
    int Points,
    int Reliability,
    DateTime CreatedAt,
    DateTime UpdatedAt
)
;
