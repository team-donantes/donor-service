using Donnum.DonorService.Application.Features.Donors.Dtos;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorByAuthUserId;

public sealed record GetDonorByAuthUserIdQuery(Guid AuthUserId) : IRequest<Guid>;
