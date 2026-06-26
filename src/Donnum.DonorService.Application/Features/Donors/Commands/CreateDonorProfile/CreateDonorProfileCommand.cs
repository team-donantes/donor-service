using MediatR;

using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed record CreateDonorProfileCommand(
    Guid AuthUserId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string BloodGroup,
    string RhFactor,
    Gender Gender,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude,
    int Age = 18,
    decimal WeightKg = 50,
    bool IsHealthy = true,
    bool IsPregnant = false,
    bool HasGuardianAuthorization = true,
    bool HasRecentTattooOrPiercing = false,
    bool HasMedicalRestriction = false
) : IRequest<Guid>;
