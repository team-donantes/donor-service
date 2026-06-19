using System;
using System.Collections.Generic;
using System.Linq;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public static class DonationMapper
{
    public static DonorDonationHistoryDto MapToDto(Guid donorId, List<Donation> donations)
    {
        return new DonorDonationHistoryDto(
            donorId,
            donations.Select(d => new DonationHistoryItemDto(
                d.Id,
                d.DonationRequestId,
                d.MedicalCenterId,
                d.DonationDate,
                d.CreatedAt
            )).ToList()
        );
    }
}
