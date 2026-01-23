using Agronomia.Api.Features.Marketplace.GetMarketplaceProductOffers;
using Agronomia.Api.Features.Marketplace.SearchMarketplaceProducts;

namespace Agronomia.Api.Features.Marketplace;

public static class MarketplaceEndpoints
{
    public static void MapMarketplaceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSearchMarketplaceProducts();
        app.MapGetMarketplaceProductOffers();
    }
}
