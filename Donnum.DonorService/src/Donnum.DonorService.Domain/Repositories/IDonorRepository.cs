using System;
using System.Threading;
using System.Threading.Tasks;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonorRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
