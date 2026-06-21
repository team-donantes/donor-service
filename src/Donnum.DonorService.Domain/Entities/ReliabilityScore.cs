using System;
using Donnum.DonorService.Domain.Common;

namespace Donnum.DonorService.Domain.Entities;

public class ReliabilityScore : Entity
{
    public Guid DonorId { get; set; }
    public int Score { get; set; }
    public DateTime LastCalculatedAt { get; set; }

    public Donor Donor { get; set; } = null!;
}
