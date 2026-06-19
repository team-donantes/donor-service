using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Infrastructure.Data.Configurations;

public class DonationRequestParticipationConfiguration : IEntityTypeConfiguration<DonationRequestParticipation>
{
    public void Configure(EntityTypeBuilder<DonationRequestParticipation> builder)
    {
        builder.HasKey(x => x.Id);

       
        builder.HasIndex(x => new { x.DonorId, x.DonationRequestId }).IsUnique();
    }
}
