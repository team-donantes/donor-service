using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Repositories;

namespace Donnum.DonorService.Application.Features.Donors.Services;

public class EvaluateAndAssignBadgesService(
    IDonorRepository donorRepository,
    IBadgeRepository badgeRepository) : IEvaluateAndAssignBadgesService
{
    public async Task ExecuteAsync(Guid donorId, CancellationToken cancellationToken = default)
    {
        var donor = await donorRepository.GetByIdAsync(donorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Donor), donorId);

        var existingBadges = await donorRepository.GetBadgesByDonorIdAsync(donor.Id, cancellationToken);
        var existingBadgeIds = existingBadges.Select(db => db.BadgeId).ToHashSet();

        var allBadges = await badgeRepository.GetAllAsync(cancellationToken);

        var qualifyingBadges = allBadges
            .Where(b => donor.Points >= b.BadgePoints && !existingBadgeIds.Contains(b.Id))
            .ToList();

        if (qualifyingBadges.Count > 0)
        {
            foreach (var badge in qualifyingBadges)
            {
                var donorBadge = new DonorBadge
                {
                    DonorId = donor.Id,
                    BadgeId = badge.Id,
                    AssignedAt = DateTime.UtcNow
                };
                await donorRepository.AddDonorBadgeAsync(donorBadge, cancellationToken);
            }
            await donorRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
