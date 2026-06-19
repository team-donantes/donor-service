using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;

public class GetDonationHistoryQueryValidator : AbstractValidator<GetDonationHistoryQuery>
{
    public GetDonationHistoryQueryValidator()
    {
        RuleFor(x => x.DonorId)
            .NotEmpty().WithMessage("El ID del donante es requerido.");
    }
}
