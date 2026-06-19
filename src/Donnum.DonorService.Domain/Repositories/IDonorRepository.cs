using Donnum.DonorService.Domain.Entities;
using System.Linq.Expressions;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonorRepository
{
    Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> GetBadgesByDonorIdAsync<TResult>(Guid donorId, Expression<Func<DonorBadge, TResult>> selector, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default);

    Task AddAsync(Donor donor, CancellationToken cancellationToken = default);

    void Update(Donor donor);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
