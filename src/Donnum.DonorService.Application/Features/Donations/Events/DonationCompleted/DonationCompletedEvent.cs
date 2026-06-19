using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;

public sealed record DonationCompletedEvent(
    Guid DonorId,
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    DateTime CreatedAt
) : IRequest;
