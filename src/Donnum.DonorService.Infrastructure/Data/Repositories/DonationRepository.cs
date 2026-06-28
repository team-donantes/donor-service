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

    public async Task AddAsync(Donation donation, CancellationToken cancellationToken)
        => await context.Donations.AddAsync(donation, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);

    public async Task AddParticipationAsync(DonationRequestParticipation participation, CancellationToken cancellationToken = default)
        => await context.DonationRequestParticipations.AddAsync(participation, cancellationToken);

    public async Task<DonationRequestParticipation?> GetParticipationAsync(Guid donorId, Guid requestId, CancellationToken cancellationToken = default)
    {
        return await context.DonationRequestParticipations
            .FirstOrDefaultAsync(p => p.DonorId == donorId && p.DonationRequestId == requestId, cancellationToken);
    }

    public async Task<List<DonationRequestParticipation>> GetParticipationsByDonorIdAsync(Guid donorId, CancellationToken cancellationToken = default)
    {
        return await context.DonationRequestParticipations
            .AsNoTracking()
            .Where(p => p.DonorId == donorId)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsCampaignRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        context.BloodRequestProjections.AnyAsync(
            request => request.Id == requestId && request.RequestType == "Campaign",
            cancellationToken);
}
