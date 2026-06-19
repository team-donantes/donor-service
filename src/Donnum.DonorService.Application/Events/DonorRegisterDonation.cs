namespace Donnum.DonorService.Application.Events;

public sealed record DonorRegisterDonation(
    Guid DonorId,
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    DateTime CreatedAt
);
