using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed record CreateDonorProfileCommand(
    Guid AuthUserId,
    string BloodGroup,
    string RhFactor,
    Donnum.DonorService.Domain.Enums.Gender Gender,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude
) : IRequest<Guid>;
