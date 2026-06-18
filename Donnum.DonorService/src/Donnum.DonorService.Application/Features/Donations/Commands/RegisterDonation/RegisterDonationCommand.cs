using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Commands.RegisterDonation;

public sealed record RegisterDonationCommand(
    Guid DonorId,
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    DateTime CreatedAt
) : IRequest;
