using Agronomia.Domain.Catalog.Products;
using FluentValidation;

namespace Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

public sealed class SearchMarketplaceProductsValidator : AbstractValidator<SearchMarketplaceProductsQuery>
{
    public SearchMarketplaceProductsValidator()
    {
        RuleFor(query => query.FarmId)
            .NotEmpty();

        RuleFor(query => query.ExecutorUserId)
            .NotEmpty();

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
