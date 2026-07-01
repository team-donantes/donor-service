namespace Donnum.DonorService.Presentation.API.Contracts;

public sealed record UpdateDonorProfileRequest(
    string? Street,
    string? PhoneNumber,
    string City,
    string Province
);
