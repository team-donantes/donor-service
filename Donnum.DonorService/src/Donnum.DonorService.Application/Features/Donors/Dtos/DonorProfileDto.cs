using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Dtos;

public sealed record DonorProfileDto(
    Guid Id,
    Guid AuthUserId,
    string BloodGroup,
    string RhFactor,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude,
    int ReliabilityScore,
    DateTime CreatedAt,
    DateTime UpdatedAt
)
{
    public static DonorProfileDto FromDonor(Donor donor)
        => new(
            Id: donor.Id,
            AuthUserId: donor.AuthUserId,
            BloodGroup: donor.BloodGroup,
            RhFactor: donor.RhFactor,
            Street: donor.Street,
            City: donor.City,
            Province: donor.Province,
            Latitude: donor.Latitude,
            Longitude: donor.Longitude,
            ReliabilityScore: donor.ReliabilityScore,
            CreatedAt: donor.CreatedAt,
            UpdatedAt: donor.UpdatedAt
        );
}
