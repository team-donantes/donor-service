using System;
using System.Collections.Generic;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public record DonationHistoryItemDto(
    Guid Id,
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    DateTime CreatedAt);

public record DonorDonationHistoryDto(
    Guid DonorId,
    List<DonationHistoryItemDto> Donations);
