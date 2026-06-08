using System;

namespace Donnum.DonorService.Domain.Entities;

public class Donation
{
    public Guid Id { get; set; }
    public Guid DonorId { get; set; }
    public Guid DonationRequestId { get; set; }
    public Guid MedicalCenterId { get; set; }
    public DateTime DonationDate { get; set; }
    public DateTime CreatedAt { get; set; }

    
    public Donor Donor { get; set; } = null!;
}
