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
}
