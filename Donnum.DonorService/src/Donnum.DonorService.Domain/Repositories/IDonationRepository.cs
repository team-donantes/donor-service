using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonationRepository
{
    Task<List<Donation>?> GetByDonorIdAsync(Guid donorId, CancellationToken cancellationToken);
    Task AddAsync(Donation donation, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
