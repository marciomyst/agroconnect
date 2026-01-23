namespace Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

public sealed record MarketplaceProductListItemDto(
    Guid ProductId,
    string Name,
    string Category,
    bool IsControlledByRecipe,
    decimal BestPrice,
    bool HasAvailableOffers);
