using System;
using System.Collections.Generic;
using System.Linq;
using Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;
using Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donations.Mappers;

public static class DonationMapper
{
    public static Donation ToEntity(DonationCompletedEvent @event)
    {
        return new Donation
        {
            DonorId = @event.DonorId,
            DonationRequestId = @event.DonationRequestId,
            MedicalCenterId = @event.MedicalCenterId,
            DonationDate = @event.DonationDate,
            CreatedAt = @event.CreatedAt
        };
    }

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
