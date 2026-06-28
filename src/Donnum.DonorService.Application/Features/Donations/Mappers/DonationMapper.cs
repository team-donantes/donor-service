using System;
using System.Collections.Generic;
using System.Linq;

using Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donations.Mappers;

public static class DonationMapper
{
    public static Donation ToEntity(Donnum.DonorService.Application.Features.Donors.Commands.RegisterAttendance.RegisterAttendanceCommand command)
    {
        return new Donation
        {
            DonorId = command.DonorId,
            DonationRequestId = command.DonationRequestId,
            MedicalCenterId = command.MedicalCenterId,
            DonationDate = command.DonationDate,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static DonorDonationHistoryDto MapToDto(Guid donorId, List<Donation> donations, List<DonationRequestParticipation> participations)
    {
        return new DonorDonationHistoryDto(
            donorId,
            donations.Select(d => new DonationHistoryItemDto(
                d.Id,
                d.DonationRequestId,
                d.MedicalCenterId,
                d.DonationDate,
                d.CreatedAt
            )).ToList(),
            participations.Select(p => new ParticipationDto(
                p.Id,
                p.DonationRequestId,
                (int)p.Status,
                p.RegisteredAt
            )).ToList()
        );
    }
}
