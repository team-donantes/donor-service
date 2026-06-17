using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Queries.GetDonorBadges;

public sealed class GetDonorBadgesQueryValidator : AbstractValidator<GetDonorBadgesQuery>
{
    public GetDonorBadgesQueryValidator()
    {
        RuleFor(x => x.DonorId)
            .NotEmpty().WithMessage("El identificador del donante es obligatorio.");
    }
}
