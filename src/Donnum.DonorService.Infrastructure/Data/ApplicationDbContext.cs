using System.Reflection;
using Donnum.DonorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Donnum.BuildingBlocks.Messaging.Abstractions;

namespace Donnum.DonorService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<ReliabilityScore> ReliabilityScores => Set<ReliabilityScore>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<DonationRequestParticipation> DonationRequestParticipations => Set<DonationRequestParticipation>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<DonorBadge> DonorBadges => Set<DonorBadge>();
    public DbSet<BloodRequestProjection> BloodRequestProjections => Set<BloodRequestProjection>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<BloodRequestProjection>(builder =>
        {
            builder.ToTable("BloodRequestProjections");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RequestType).HasMaxLength(20).IsRequired();
        });
        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExchangeName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Topic).HasMaxLength(150).IsRequired();
            builder.Property(x => x.CorrelationId).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Payload).IsRequired();
            builder.Property(x => x.LastError).HasMaxLength(2000);
            builder.HasIndex(x => new { x.PublishedAt, x.NextAttemptAt });
        });
        modelBuilder.Entity<InboxMessage>(builder =>
        {
            builder.ToTable("InboxMessages");
            builder.HasKey(x => x.MessageId);
            builder.Property(x => x.MessageId).HasMaxLength(100);
            builder.Property(x => x.Topic).HasMaxLength(150).IsRequired();
        });
    }
}
