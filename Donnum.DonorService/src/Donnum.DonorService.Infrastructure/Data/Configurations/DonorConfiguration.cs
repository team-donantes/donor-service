using Donnum.DonorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.ToTable("Donors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BloodGroup)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.RhFactor)
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(x => x.Street)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Province)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Latitude)
            .HasPrecision(10, 8);

        builder.Property(x => x.Longitude)
            .HasPrecision(11, 8);

        builder.Property(x => x.ReliabilityScore)
            .IsRequired();

        builder.Property(x => x.Points)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasMany(x => x.Donations)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DonationRequestParticipations)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DonorBadges)
            .WithOne(x => x.Donor)
            .HasForeignKey(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AuthUserId)
            .IsUnique();
            builder.HasData(new Donor
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AuthUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            BloodGroup = "O",
            RhFactor = "Positive",
            Street = "Av. Corrientes 1234",
            City = "Buenos Aires",
            Province = "Buenos Aires",
            Latitude = -34.6037m,
            Longitude = -58.3816m,
            ReliabilityScore = 100,
            Points = 0,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }

}
