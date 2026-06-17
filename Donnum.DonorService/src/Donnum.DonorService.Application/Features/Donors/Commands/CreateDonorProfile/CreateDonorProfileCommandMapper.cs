using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public static class CreateDonorProfileCommandMapper
{
    public static Donor ToEntity(this CreateDonorProfileCommand command)
        => new Donor
        {
            Id = Guid.NewGuid(),
            AuthUserId = command.AuthUserId,
            BloodGroup = command.BloodGroup.Trim().ToUpperInvariant(),
            RhFactor = command.RhFactor.Trim(),
            Street = command.Street?.Trim(),
            City = command.City.Trim(),
            Province = command.Province.Trim(),
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            ReliabilityScore = 100,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
}
