using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donors.Dtos;

public sealed record DonorProfileDto(
    Guid Id,
    Guid AuthUserId,
    string PhoneNumber,
    string BloodGroup,
    string RhFactor,
    Gender Gender,
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
