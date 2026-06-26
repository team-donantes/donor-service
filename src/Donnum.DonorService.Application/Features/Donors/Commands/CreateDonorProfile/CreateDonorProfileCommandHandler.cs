using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using MediatR;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed class CreateDonorProfileCommandHandler : IRequestHandler<CreateDonorProfileCommand, Guid>
{
    private readonly IDonorRepository _donorRepository;
    private readonly IIntegrationEventOutbox _outbox;

    public CreateDonorProfileCommandHandler(IDonorRepository donorRepository, IIntegrationEventOutbox outbox)
    {
        _donorRepository = donorRepository;
        _outbox = outbox;
    }

    public async Task<Guid> Handle(CreateDonorProfileCommand request, CancellationToken cancellationToken)
    {
        if (await _donorRepository.ExistsByAuthUserIdAsync(request.AuthUserId, cancellationToken))
            throw new DomainException($"Ya existe un perfil de donante para el usuario '{request.AuthUserId}'.");

        var donor = DonorMapper.ToEntity(request);

        await _donorRepository.AddAsync(donor, cancellationToken);
        await EnqueueProfileAsync(donor, cancellationToken);
        await _donorRepository.SaveChangesAsync(cancellationToken);

        return donor.Id;
    }

    private Task EnqueueProfileAsync(Donor donor, CancellationToken cancellationToken)
    {
        var eventId = Guid.NewGuid();
        return _outbox.EnqueueAsync(
            Exchanges.Donor,
            DonnumEventTopics.DonorProfileUpserted,
            new
            {
                EventId = eventId,
                OccurredAt = DateTimeOffset.UtcNow,
                DonorId = donor.Id,
                donor.AuthUserId,
                donor.FirstName,
                donor.LastName,
                donor.PhoneNumber,
                BloodType = $"{donor.BloodGroup}{(donor.RhFactor == "+" || donor.RhFactor.StartsWith("P", StringComparison.OrdinalIgnoreCase) ? "+" : "-")}",
                Gender = donor.Gender.ToString(),
                donor.City,
                Latitude = donor.Location.Latitude,
                Longitude = donor.Location.Longitude,
                donor.Age,
                donor.WeightKg,
                donor.IsHealthy,
                donor.IsPregnant,
                donor.HasGuardianAuthorization,
                donor.HasRecentTattooOrPiercing,
                donor.HasMedicalRestriction,
                DonationsThisYear = 0,
                LastDonationDate = (DateTime?)null
            },
            donor.AuthUserId.ToString("D"),
            eventId,
            cancellationToken);
    }
}
