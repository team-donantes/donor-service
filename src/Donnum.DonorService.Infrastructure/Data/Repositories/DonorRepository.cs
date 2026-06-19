using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<IReadOnlyList<TResult>> GetBadgesByDonorIdAsync<TResult>(Guid donorId, Expression<Func<DonorBadge, TResult>> selector, CancellationToken cancellationToken = default)
        => await _context.Donors
            .AsNoTracking()
            .Where(d => d.Id == donorId)
            .SelectMany(d => d.DonorBadges)
            .Select(selector)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default)
        => await _context.Donors
            .AsNoTracking()
            .AnyAsync(d => d.AuthUserId == authUserId, cancellationToken);

    public async Task AddAsync(Donor donor, CancellationToken cancellationToken = default)
        => await _context.Donors.AddAsync(donor, cancellationToken);

    public void Update(Donor donor)
        => _context.Donors.Update(donor);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
