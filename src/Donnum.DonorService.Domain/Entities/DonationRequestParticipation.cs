using System;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Domain.Entities;

public class DonationRequestParticipation
{
    public Guid Id { get; set; }
    public Guid DonorId { get; set; }
    public Guid DonationRequestId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ParticipationStatus Status { get; set; }

  
    public Donor Donor { get; set; } = null!;
}
