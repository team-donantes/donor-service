using System;
using Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.ValueObjects;

namespace Donnum.DonorService.Application.Features.Donors.Mappers;

public static class DonorMapper
{
    public static Donor ToEntity(CreateDonorProfileCommand command)
        => new Donor
        {
            AuthUserId = command.AuthUserId,
            BloodGroup = command.BloodGroup.Trim().ToUpperInvariant(),
            RhFactor = command.RhFactor.Trim(),
            Street = command.Street?.Trim(),
            City = command.City.Trim(),
            Province = command.Province.Trim(),
            Location = new Location(command.Latitude, command.Longitude),
            Points = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    public static void ApplyUpdate(UpdateDonorProfileCommand command, Donor donor)
    {
        donor.Street = command.Street?.Trim();
        donor.City = command.City.Trim();
        donor.Province = command.Province.Trim();
        donor.Location = new Location(command.Latitude, command.Longitude);
        donor.UpdatedAt = DateTime.UtcNow;
    }

    public static DonorProfileDto ToDto(Donor donor)
        => new(
            Id: donor.Id,
            AuthUserId: donor.AuthUserId,
            BloodGroup: donor.BloodGroup,
            RhFactor: donor.RhFactor,
            Street: donor.Street,
            City: donor.City,
            Province: donor.Province,
            Latitude: donor.Location.Latitude,
            Longitude: donor.Location.Longitude,
            Points: donor.Points,
            CreatedAt: donor.CreatedAt,
            UpdatedAt: donor.UpdatedAt
        );
}
