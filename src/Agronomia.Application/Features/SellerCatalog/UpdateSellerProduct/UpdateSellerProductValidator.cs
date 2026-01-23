using Agronomia.Domain.Catalog.ValueObjects;
using FluentValidation;

namespace Agronomia.Application.Features.SellerCatalog.UpdateSellerProduct;

public sealed class UpdateSellerProductValidator : AbstractValidator<UpdateSellerProductCommand>
{
    public UpdateSellerProductValidator()
    {
        RuleFor(command => command.SellerId)
            .NotEmpty();

        RuleFor(command => command.ExecutorUserId)
            .NotEmpty();

        RuleFor(command => command.SellerProductId)
            .NotEmpty();

        RuleFor(command => command.Price)
            .GreaterThan(0);

        RuleFor(command => command.Currency)
            .NotEmpty()
            .Must(BeValidCurrency)
            .WithMessage("Currency is invalid.");
    }

    private static bool BeValidCurrency(string value)
        => Enum.TryParse<Currency>(value, ignoreCase: true, out _);
}
