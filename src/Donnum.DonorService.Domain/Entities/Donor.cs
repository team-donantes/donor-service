using System;
using System.Collections.Generic;

using Donnum.DonorService.Domain.Common;
using Donnum.DonorService.Domain.ValueObjects;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Domain.Entities;

public class Donor : Entity
{
    public Guid AuthUserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public decimal WeightKg { get; set; }
    public bool IsHealthy { get; set; } = true;
    public bool IsPregnant { get; set; }
    public bool HasGuardianAuthorization { get; set; }
    public bool HasRecentTattooOrPiercing { get; set; }
    public bool HasMedicalRestriction { get; set; }

    public string BloodGroup { get; set; } = string.Empty;
    public string RhFactor { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public Location Location { get; set; } = null!;
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Donation> Donations { get; set; } = [];
    public ICollection<DonationRequestParticipation> DonationRequestParticipations { get; set; } = [];
    public ICollection<DonorBadge> DonorBadges { get; set; } = [];
    public ReliabilityScore? ReliabilityScore { get; set; }

    public void UpdateReliabilityScore(int newScore)
    {
        if (ReliabilityScore != null)
        {
            ReliabilityScore.Score = newScore;
            ReliabilityScore.LastCalculatedAt = DateTime.UtcNow;
        }
    }
}
