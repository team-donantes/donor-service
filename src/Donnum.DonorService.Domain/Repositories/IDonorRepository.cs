using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonorRepository
{
    Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DonorBadge>> GetBadgesByDonorIdAsync(Guid donorId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken = default);

    Task<Donor?> GetWithReliabilityScoreByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Donor donor, CancellationToken cancellationToken = default);

    Task AddReliabilityScoreAsync(ReliabilityScore score, CancellationToken cancellationToken = default);

    void Update(Donor donor);

    Task AddDonorBadgeAsync(DonorBadge donorBadge, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

