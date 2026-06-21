using Donnum.DonorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class ReliabilityScoreConfiguration : IEntityTypeConfiguration<ReliabilityScore>
{
    public void Configure(EntityTypeBuilder<ReliabilityScore> builder)
    {
        builder.ToTable("ReliabilityScores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score)
            .IsRequired();

        builder.Property(x => x.LastCalculatedAt)
            .IsRequired();


        builder.HasOne(x => x.Donor)
            .WithOne(x => x.ReliabilityScore)
            .HasForeignKey<ReliabilityScore>(x => x.DonorId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasData(new
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            DonorId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Score = 100,
            LastCalculatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        });
    }
}
