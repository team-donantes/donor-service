using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddPoints;

public sealed class AddPointsCommandValidator : AbstractValidator<AddPointsCommand>
{
    public AddPointsCommandValidator()
    {
        RuleFor(x => x.DonorId)
            .NotEmpty().WithMessage("El identificador del donante es obligatorio.");

        RuleFor(x => x.Points)
            .GreaterThan(0).WithMessage("La cantidad de puntos debe ser mayor a cero.");
    }
}
