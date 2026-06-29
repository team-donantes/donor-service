using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Donnum.DonorService.Application.Features.Donations.Commands.CreateParticipation;

public sealed class CreateParticipationCommandHandler : IRequestHandler<CreateParticipationCommand>
{
    private readonly IDonationRepository _donationRepository;
    private readonly IDonorRepository _donorRepository;

    public CreateParticipationCommandHandler(IDonationRepository donationRepository, IDonorRepository donorRepository)
    {
        _donationRepository = donationRepository;
        _donorRepository = donorRepository;
    }

    public async Task Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetByIdAsync(request.DonorId, cancellationToken);
        if (donor == null)
            throw new DomainException($"No se encontró el donante con ID {request.DonorId}");

        var donorType = $"{donor.BloodGroup}{donor.RhFactor}";
        if (!IsCompatible(donorType, request.RequestedBloodTypes))
            throw new DomainException($"Tu tipo de sangre ({donorType}) no es compatible con los tipos requeridos por esta urgencia.");

        var existingParticipation = await _donationRepository.GetParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
        if (existingParticipation != null)
        {
            existingParticipation.Status = request.Status;
            existingParticipation.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var participation = new DonationRequestParticipation
            {
                DonorId = request.DonorId,
                DonationRequestId = request.DonationRequestId,
                Status = request.Status,
                RegisteredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _donationRepository.AddParticipationAsync(participation, cancellationToken);
        }

        await _donationRepository.SaveChangesAsync(cancellationToken);
    }

    private static bool IsCompatible(string donorType, List<string> requestedTypes)
    {
        var canDonateTo = donorType switch
        {
            "A+"  => new[] { "A+", "AB+" },
            "A-"  => new[] { "A+", "A-", "AB+", "AB-" },
            "B+"  => new[] { "B+", "AB+" },
            "B-"  => new[] { "B+", "B-", "AB+", "AB-" },
            "AB+" => new[] { "AB+" },
            "AB-" => new[] { "AB+", "AB-" },
            "O+"  => new[] { "A+", "B+", "AB+", "O+" },
            "O-"  => new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" },
            _     => Array.Empty<string>()
        };
        return requestedTypes.Any(canDonateTo.Contains);
    }
}
