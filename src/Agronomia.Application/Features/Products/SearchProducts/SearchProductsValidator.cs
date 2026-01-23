using Agronomia.Domain.Catalog.Products;
using FluentValidation;

namespace Agronomia.Application.Features.Products.SearchProducts;

public sealed class SearchProductsValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);

        When(query => !string.IsNullOrWhiteSpace(query.Category), () =>
        {
            RuleFor(query => query.Category!)
                .Must(BeValidCategory)
                .WithMessage("Category is invalid.");
        });
    }

    private static bool BeValidCategory(string value)
        => Enum.TryParse<ProductCategory>(value, ignoreCase: true, out _);
}
