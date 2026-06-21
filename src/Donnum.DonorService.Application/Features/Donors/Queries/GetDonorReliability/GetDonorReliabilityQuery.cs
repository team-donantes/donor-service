using System;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorReliability;

public sealed record GetDonorReliabilityQuery(Guid DonorId) : IRequest<DonorReliabilityDto>;
