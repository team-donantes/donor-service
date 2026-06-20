using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Donnum.DonorService.Infrastructure.Data.Repositories;

public class BadgeRepository(ApplicationDbContext context) : IBadgeRepository
{
    public async Task<IReadOnlyList<Badge>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Badges.AsNoTracking().ToListAsync(cancellationToken);
    }
}
