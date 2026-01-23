using Agronomia.Domain.Catalog.Products;
using FluentValidation;

namespace Agronomia.Application.Features.Products.CreateProduct;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(ProductName.MaxLength);

        RuleFor(command => command.Category)
            .NotEmpty()
            .Must(BeValidCategory)
            .WithMessage("Category is invalid.");

        RuleFor(command => command.UnitOfMeasure)
            .NotEmpty()
            .Must(BeValidUnitOfMeasure)
            .WithMessage("UnitOfMeasure is invalid.");

        RuleFor(command => command.RegistrationNumber)
            .MaximumLength(RegistrationNumber.MaxLength)
            .When(command => !string.IsNullOrWhiteSpace(command.RegistrationNumber));
    }

    private static bool BeValidCategory(string value)
        => Enum.TryParse<ProductCategory>(value, ignoreCase: true, out _);

    private static bool BeValidUnitOfMeasure(string value)
        => Enum.TryParse<UnitOfMeasure>(value, ignoreCase: true, out _);
}
