using MediatR;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CancelParticipation;

public record CancelParticipationCommand(Guid DonorId, Guid DonationRequestId) : IRequest;
