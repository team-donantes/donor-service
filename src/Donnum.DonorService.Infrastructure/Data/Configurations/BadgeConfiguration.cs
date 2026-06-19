using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

      
        builder.HasIndex(x => x.BadgeType).IsUnique();

        
        builder.HasData(
            new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Donante Donor", BadgeType = BadgeType.Silver, BadgePoints = 50, IsDeleted = false },
            new { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Frecuente", BadgeType = BadgeType.Gold, BadgePoints = 100, IsDeleted = false },
            new { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Comprometido", BadgeType = BadgeType.Platinum, BadgePoints = 250, IsDeleted = false },
            new { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Heroe", BadgeType = BadgeType.Diamond, BadgePoints = 500, IsDeleted = false }
        );
    }
}
