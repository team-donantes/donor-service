using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Domain.Repositories;

public interface IBadgeRepository
{
    Task<IReadOnlyList<Badge>> GetAllAsync(CancellationToken cancellationToken = default);
}
