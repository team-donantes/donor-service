using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public sealed record UpdateDonorProfileCommand(
    Guid Id,
    string? Street,
    string? PhoneNumber,
    string City,
    string Province
) : IRequest;
