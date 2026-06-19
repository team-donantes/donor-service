using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorProfile;

public sealed class GetDonorProfileQueryValidator : AbstractValidator<GetDonorProfileQuery>
{
    public GetDonorProfileQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El identificador del donante es obligatorio.");
    }
}
