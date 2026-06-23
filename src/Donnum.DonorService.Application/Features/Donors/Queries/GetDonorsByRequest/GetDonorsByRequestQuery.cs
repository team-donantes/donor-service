using Donnum.DonorService.Application.Features.Donors.Dtos;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorsByRequest;

public record GetDonorsByRequestQuery(Guid RequestId) : IRequest<IReadOnlyList<DonorProfileDto>>;
