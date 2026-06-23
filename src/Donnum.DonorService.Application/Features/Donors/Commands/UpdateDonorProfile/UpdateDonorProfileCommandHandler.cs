using Donnum.DonorService.Application.Exceptions;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public sealed class UpdateDonorProfileCommandHandler : IRequestHandler<UpdateDonorProfileCommand>
{
    private readonly IDonorRepository _donorRepository;

    public UpdateDonorProfileCommandHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
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
        await _donorRepository.SaveChangesAsync(cancellationToken);
    }
}
