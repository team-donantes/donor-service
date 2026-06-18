using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public static class UpdateDonorProfileCommandMapper
{
    public static void ApplyTo(this UpdateDonorProfileCommand command, Donor donor)
    {
        donor.Street = command.Street?.Trim();
        donor.City = command.City.Trim();
        donor.Province = command.Province.Trim();
        donor.Latitude = command.Latitude;
        donor.Longitude = command.Longitude;
        donor.UpdatedAt = DateTime.UtcNow;
    }
}
