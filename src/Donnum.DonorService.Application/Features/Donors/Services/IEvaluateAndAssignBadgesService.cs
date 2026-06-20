using System;
using System.Threading;
using System.Threading.Tasks;

namespace Donnum.DonorService.Application.Features.Donors.Services;

public interface IEvaluateAndAssignBadgesService
{
    Task ExecuteAsync(Guid donorId, CancellationToken cancellationToken = default);
}
