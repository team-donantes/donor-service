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

        builder.HasData(new
        {
            Id = Guid.Parse("e0000000-e000-e000-e000-e00000000001"),
            DonorId = Guid.Parse("d0000000-d000-d000-d000-d00000000001"),
            DonationRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MedicalCenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            DonationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        });
    }
}
