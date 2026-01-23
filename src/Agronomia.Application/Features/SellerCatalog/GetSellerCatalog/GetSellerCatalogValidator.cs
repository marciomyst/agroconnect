using FluentValidation;

namespace Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

public sealed class GetSellerCatalogValidator : AbstractValidator<GetSellerCatalogQuery>
{
    public GetSellerCatalogValidator()
    {
        RuleFor(query => query.SellerId)
            .NotEmpty();

        RuleFor(query => query.ExecutorUserId)
            .NotEmpty();

        RuleFor(query => query.Page)
            .GreaterThan(0);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);
    }
}
