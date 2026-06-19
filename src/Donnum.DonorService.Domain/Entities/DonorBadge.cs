using System;

using Donnum.DonorService.Domain.Common;

namespace Donnum.DonorService.Domain.Entities;

public class DonorBadge : Entity
{
    public Guid BadgeId { get; set; }
    public Guid DonorId { get; set; }
    public DateTime AssignedAt { get; set; }

  
    public Badge Badge { get; set; } = null!;
    public Donor Donor { get; set; } = null!;
}
