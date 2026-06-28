using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Domain.Repositories;

public interface IDonationRepository
{
    Task<List<Donation>> GetByDonorIdAsync(Guid donorId, CancellationToken cancellationToken);
    Task AddAsync(Donation donation, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task AddParticipationAsync(DonationRequestParticipation participation, CancellationToken cancellationToken = default);
    Task<DonationRequestParticipation?> GetParticipationAsync(Guid donorId, Guid requestId, CancellationToken cancellationToken = default);
    Task<List<DonationRequestParticipation>> GetParticipationsByDonorIdAsync(Guid donorId, CancellationToken cancellationToken = default);
    Task<bool> IsCampaignRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}
