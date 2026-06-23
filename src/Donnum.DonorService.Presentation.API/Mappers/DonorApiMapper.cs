using Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;
using Donnum.DonorService.Presentation.API.Contracts;

namespace Donnum.DonorService.Presentation.API.Mappers;

public static class DonorApiMapper
{
    public static UpdateDonorProfileCommand ToCommand(Guid id, UpdateDonorProfileRequest request)
        => new UpdateDonorProfileCommand(
            Id: id,
            Street: request.Street,
            City: request.City,
            Province: request.Province
        );
}
