using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Donnum.DonorService.Infrastructure.Data.Repositories;

public sealed class DonorRepository : IDonorRepository
{
    private readonly ApplicationDbContext _context;

    public DonorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Donors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Donors
            .AsNoTracking()
            .AnyAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DonorBadge>> GetBadgesByDonorIdAsync(Guid donorId, CancellationToken cancellationToken = default)
        => await _context.DonorBadges
            .AsNoTracking()
            .Include(db => db.Badge)
            .Where(db => db.DonorId == donorId)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default)
        => await _context.Donors
            .AsNoTracking()
            .AnyAsync(d => d.AuthUserId == authUserId, cancellationToken);

    public async Task<IReadOnlyList<Donor>> GetDonorsByRequestIdAsync(Guid requestId, CancellationToken cancellationToken = default)
        => await _context.DonationRequestParticipations
            .AsNoTracking()
            .Where(p => p.DonationRequestId == requestId)
            .Select(p => p.Donor)
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<Donor?> GetWithReliabilityScoreByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Donor> query = _context.Donors.Include(d => d.ReliabilityScore);
        
        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }
        
        return await query.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task AddAsync(Donor donor, CancellationToken cancellationToken = default)
        => await _context.Donors.AddAsync(donor, cancellationToken);

    public async Task AddReliabilityScoreAsync(ReliabilityScore score, CancellationToken cancellationToken = default)
        => await _context.ReliabilityScores.AddAsync(score, cancellationToken);

    public void Update(Donor donor)
        => _context.Donors.Update(donor);

    public async Task AddDonorBadgeAsync(DonorBadge donorBadge, CancellationToken cancellationToken = default)
        => await _context.DonorBadges.AddAsync(donorBadge, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
