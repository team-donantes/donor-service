using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using MediatR;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public sealed class UpdateDonorProfileCommandHandler : IRequestHandler<UpdateDonorProfileCommand>
{
    private readonly IDonorRepository _donorRepository;
    private readonly IIntegrationEventOutbox _outbox;

    public UpdateDonorProfileCommandHandler(IDonorRepository donorRepository, IIntegrationEventOutbox outbox)
    {
        _donorRepository = donorRepository;
        _outbox = outbox;
    }

    public async Task Handle(UpdateDonorProfileCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Donor), request.Id);

        DonorMapper.ApplyUpdate(request, donor);

        try 
        {
            var locations = await Microsoft.Maui.Devices.Sensors.Geocoding.Default.GetLocationsAsync($"{request.Street}, {request.City}, {request.Province}");
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                donor.Location = new Domain.ValueObjects.Location((decimal)location.Latitude, (decimal)location.Longitude);
            }
        } 
        catch (Exception)
        {
        
        }

        _donorRepository.Update(donor);
        var eventId = Guid.NewGuid();
        await _outbox.EnqueueAsync(
            Exchanges.Donor,
            DonnumEventTopics.DonorProfileUpserted,
            new
            {
                EventId = eventId,
                OccurredAt = DateTimeOffset.UtcNow,
                DonorId = donor.Id,
                donor.AuthUserId,
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
        await _donorRepository.SaveChangesAsync(cancellationToken);
    }
}
