using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;
using Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

namespace Agronomia.Application.Abstractions.Catalog;

public interface IMarketplaceReadRepository
{
    Task<PagedResult<MarketplaceProductListItemDto>> SearchProductsAsync(
        MarketplaceSearchCriteria criteria,
        CancellationToken ct);

    Task<IReadOnlyList<MarketplaceProductOfferItemDto>> GetProductOffersAsync(
        Guid productId,
        CancellationToken ct);
}
