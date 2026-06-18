using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonorRepository
{
    Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Donor?> GetByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default);

    Task AddAsync(Donor donor, CancellationToken cancellationToken = default);

    void Update(Donor donor);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
