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

        builder.HasData(new
        {
            Id = Guid.Parse("b0000000-b000-b000-b000-b00000000001"),
            DonorId = Guid.Parse("d0000000-d000-d000-d000-d00000000001"),
            BadgeId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            AssignedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        });
    }
}
