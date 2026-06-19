using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.HasKey(x => x.Id);
        
        
        builder.HasIndex(x => x.DonorId);
        builder.HasIndex(x => x.DonationRequestId);
        builder.HasIndex(x => x.MedicalCenterId);
    }
}
