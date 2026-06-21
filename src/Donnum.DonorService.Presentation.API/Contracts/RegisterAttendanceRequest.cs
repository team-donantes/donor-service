using System;

namespace Donnum.DonorService.Presentation.API.Contracts;

public sealed record RegisterAttendanceRequest(
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    bool Attended
);
