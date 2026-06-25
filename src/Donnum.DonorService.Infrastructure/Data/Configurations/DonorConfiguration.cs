using Donnum.DonorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Donnum.DonorService.Domain.ValueObjects;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
    private const byte LatitudePrecision = 10, LatitudeScale = 8;
    private const byte LongitudePrecision = 11, LongitudeScale = 8;

    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.ToTable("Donors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Gender).IsRequired();

        builder.Property(x => x.BloodGroup).HasMaxLength(3).IsRequired();
        builder.Property(x => x.RhFactor).HasMaxLength(15).IsRequired();
        builder.Property(x => x.Street).HasMaxLength(255).IsRequired(false);
        builder.Property(x => x.City).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Province).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired(false);

        builder.OwnsOne(x => x.Location, l =>
        {
            l.Property(p => p.Latitude).HasPrecision(LatitudePrecision, LatitudeScale);
            l.Property(p => p.Longitude).HasPrecision(LongitudePrecision, LongitudeScale);
            
            l.HasData(new
            {
                DonorId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Latitude = -34.6037m,
                Longitude = -58.3816m
            });
        });

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasMany(x => x.Donations)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.DonationRequestParticipations)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DonorBadges)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AuthUserId).IsUnique();
        
        builder.HasData(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AuthUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Gender = Gender.Male,
            BloodGroup = "O",
            RhFactor = "Positive",
            Street = "Av. Corrientes 1234",
            City = "Buenos Aires",
            Province = "Buenos Aires",
            Points = 0,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        });
    }

}
