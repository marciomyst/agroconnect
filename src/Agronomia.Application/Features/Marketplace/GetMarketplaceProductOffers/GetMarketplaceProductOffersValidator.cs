using FluentValidation;

namespace Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;

public sealed class GetMarketplaceProductOffersValidator : AbstractValidator<GetMarketplaceProductOffersQuery>
{
    public GetMarketplaceProductOffersValidator()
    {
        RuleFor(query => query.FarmId)
            .NotEmpty();

        RuleFor(query => query.ExecutorUserId)
            .NotEmpty();

        RuleFor(query => query.ProductId)
            .NotEmpty();
    }
}
