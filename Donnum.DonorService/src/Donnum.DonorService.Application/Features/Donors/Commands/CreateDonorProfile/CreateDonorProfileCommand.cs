using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed record CreateDonorProfileCommand(
    Guid AuthUserId,
    string BloodGroup,
    string RhFactor,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude
) : IRequest<Guid>;
