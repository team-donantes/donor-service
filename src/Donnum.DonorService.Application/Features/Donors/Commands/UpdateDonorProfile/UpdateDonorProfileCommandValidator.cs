using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;

public sealed class UpdateDonorProfileCommandValidator : AbstractValidator<UpdateDonorProfileCommand>
{
    public UpdateDonorProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El identificador del donante es obligatorio.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100).WithMessage("La ciudad no puede superar los 100 caracteres.");

        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("La provincia es obligatoria.")
            .MaximumLength(100).WithMessage("La provincia no puede superar los 100 caracteres.");

        RuleFor(x => x.Street)
            .MaximumLength(255).WithMessage("La calle no puede superar los 255 caracteres.")
            .When(x => x.Street is not null);
    }
}
