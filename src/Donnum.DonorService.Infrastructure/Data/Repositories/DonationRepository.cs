using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Donnum.DonorService.Infrastructure.Data.Repositories;

public class DonationRepository(ApplicationDbContext context) : IDonationRepository
{
    public async Task<List<Donation>> GetByDonorIdAsync(Guid donorId, CancellationToken cancellationToken)
    {
        return await context.Donations
            .AsNoTracking()
            .Where(d => d.DonorId == donorId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Donation donation, CancellationToken cancellationToken)
        => await context.Donations.AddAsync(donation, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);
}

