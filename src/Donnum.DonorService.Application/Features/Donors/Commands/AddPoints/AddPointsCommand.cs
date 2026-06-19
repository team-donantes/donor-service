using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddPoints;

public sealed record AddPointsCommand(Guid DonorId, int Points) : IRequest;
