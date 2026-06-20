using System;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddDonorPoints;

public sealed record AddDonorPointsCommand(
    Guid DonorId,
    int Points
) : IRequest;
