namespace Donnum.DonorService.Domain.Entities;

public class Donor
{
    public Guid Id { get; set; }
    public Guid AuthUserId { get; set; }

    public string BloodGroup { get; set; } = string.Empty;
    public string RhFactor { get; set; } = string.Empty;

    public string? Street { get; set; }
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public int ReliabilityScore { get; set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    public ICollection<DonationRequestParticipation> DonationRequestParticipations { get; init; } = new List<DonationRequestParticipation>();
    public ICollection<DonorBadge> DonorBadges { get; set; } = new List<DonorBadge>();
}
