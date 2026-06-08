using System;

namespace Donnum.DonorService.Domain.Entities;

public class DonorBadge
{
    public Guid Id { get; set; }
    public Guid BadgeId { get; set; }
    public Guid DonorId { get; set; }
    public DateTime AssignedAt { get; set; }

  
    public Badge Badge { get; set; } = null!;
    public Donor Donor { get; set; } = null!;
}
