using Donnum.DonorService.Application.Features.Donors.Dtos;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorProfile;

public sealed record GetDonorProfileQuery(Guid Id) : IRequest<DonorProfileDto>;
