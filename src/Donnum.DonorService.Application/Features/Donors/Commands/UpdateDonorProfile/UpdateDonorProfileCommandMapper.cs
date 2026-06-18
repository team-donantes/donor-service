using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.ValueObjects;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public static class UpdateDonorProfileCommandMapper
{
    public static void ApplyTo(this UpdateDonorProfileCommand command, Donor donor)
    {
        donor.Street = command.Street?.Trim();
        donor.City = command.City.Trim();
        donor.Province = command.Province.Trim();
        donor.Location = new Location(command.Latitude, command.Longitude);
        donor.UpdatedAt = DateTime.UtcNow;
    }
}
