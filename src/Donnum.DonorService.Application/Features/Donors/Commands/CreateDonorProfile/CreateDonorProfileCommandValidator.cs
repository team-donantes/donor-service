using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;

public sealed class CreateDonorProfileCommandValidator : AbstractValidator<CreateDonorProfileCommand>
{
    public CreateDonorProfileCommandValidator()
    {
        RuleFor(x => x.AuthUserId)
            .NotEmpty().WithMessage("El AuthUserId es obligatorio.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.");
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+[1-9]\d{7,14}$")
            .WithMessage("El teléfono debe usar formato E.164.");

        RuleFor(x => x.BloodGroup)
            .NotEmpty().WithMessage("El grupo sanguíneo es obligatorio.")
            .MaximumLength(3).WithMessage("El grupo sanguíneo no puede superar los 3 caracteres.");

        RuleFor(x => x.RhFactor)
            .NotEmpty().WithMessage("El factor Rh es obligatorio.")
            .MaximumLength(15).WithMessage("El factor Rh no puede superar los 15 caracteres.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100).WithMessage("La ciudad no puede superar los 100 caracteres.");

        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("La provincia es obligatoria.")
            .MaximumLength(100).WithMessage("La provincia no puede superar los 100 caracteres.");

        RuleFor(x => x.Street)
            .MaximumLength(255).WithMessage("La calle no puede superar los 255 caracteres.")
            .When(x => x.Street is not null);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m).WithMessage("La latitud debe estar entre -90 y 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m).WithMessage("La longitud debe estar entre -180 y 180.");
    }
}
