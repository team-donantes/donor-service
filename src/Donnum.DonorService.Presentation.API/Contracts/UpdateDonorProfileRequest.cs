namespace Donnum.DonorService.Presentation.API.Contracts;

public sealed record UpdateDonorProfileRequest(
    string? Street,
    string? Phone,
    string City,
    string Province
);
