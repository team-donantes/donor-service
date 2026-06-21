using System;

namespace Donnum.DonorService.Application.Features.Donors.Dtos;

public sealed record DonorReliabilityDto(
    Guid DonorId,
    int Score,
    DateTime LastCalculatedAt
);
