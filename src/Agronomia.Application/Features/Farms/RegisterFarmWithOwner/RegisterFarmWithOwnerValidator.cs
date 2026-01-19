using FluentValidation;

namespace Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

public sealed class RegisterFarmWithOwnerValidator : AbstractValidator<RegisterFarmWithOwnerCommand>
{
    public RegisterFarmWithOwnerValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.TaxId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty();
    }
}
