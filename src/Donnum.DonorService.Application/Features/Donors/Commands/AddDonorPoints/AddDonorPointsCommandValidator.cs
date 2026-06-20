using FluentValidation;

namespace Donnum.DonorService.Application.Features.Donors.Commands.AddDonorPoints;

public sealed class AddDonorPointsCommandValidator : AbstractValidator<AddDonorPointsCommand>
{
    public AddDonorPointsCommandValidator()
    {
        RuleFor(x => x.DonorId)
            .NotEmpty().WithMessage("El ID del donante es obligatorio.");

        RuleFor(x => x.Points)
            .GreaterThan(0).WithMessage("Los puntos a sumar deben ser mayores que cero.");
    }
}
