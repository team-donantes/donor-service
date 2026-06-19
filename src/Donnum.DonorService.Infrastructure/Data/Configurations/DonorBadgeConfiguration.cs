using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class DonorBadgeConfiguration : IEntityTypeConfiguration<DonorBadge>
{
    public void Configure(EntityTypeBuilder<DonorBadge> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Badge)
            .WithMany()
            .HasForeignKey(x => x.BadgeId)
            .OnDelete(DeleteBehavior.Restrict);

       
        builder.HasIndex(x => new { x.DonorId, x.BadgeId }).IsUnique();
    }
}
