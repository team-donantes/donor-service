using System;
using System.Threading;
using System.Threading.Tasks;
using Donnum.DonorService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Donnum.DonorService.Infrastructure.Data.Repositories;

public class DonorRepository(ApplicationDbContext context) : IDonorRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Donors.AnyAsync(d => d.Id == id, cancellationToken);
    }
}
