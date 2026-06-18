using System;
using System.Collections.Generic;

using Donnum.DonorService.Domain.Common;
using Donnum.DonorService.Domain.ValueObjects;
namespace Donnum.DonorService.Domain.Entities;

public class Donor : Entity
{
    public Guid AuthUserId { get; set; }

    public string BloodGroup { get; set; } = string.Empty;
    public string RhFactor { get; set; } = string.Empty;

    public string? Street { get; set; }
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public Location Location { get; set; } = null!;
    public int ReliabilityScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Donation> Donations { get; set; } = [];
    public ICollection<DonationRequestParticipation> DonationRequestParticipations { get; init; } = [];
    public ICollection<DonorBadge> DonorBadges { get; set; } = [];
}
