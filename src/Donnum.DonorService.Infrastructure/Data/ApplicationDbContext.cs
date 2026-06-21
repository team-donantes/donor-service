using System.Reflection;
using Donnum.DonorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
