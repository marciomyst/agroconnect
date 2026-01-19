using FluentValidation;

namespace Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

public sealed class RegisterSellerWithOwnerValidator : AbstractValidator<RegisterSellerWithOwnerCommand>
{
    public RegisterSellerWithOwnerValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.TaxId)
            .NotEmpty();

        RuleFor(command => command.CorporateName)
            .NotEmpty();
    }
}
