using MediatR;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CreateParticipation;

public record CreateParticipationCommand(Guid DonorId, Guid DonationRequestId, ParticipationStatus Status, List<string> RequestedBloodTypes) : IRequest;
