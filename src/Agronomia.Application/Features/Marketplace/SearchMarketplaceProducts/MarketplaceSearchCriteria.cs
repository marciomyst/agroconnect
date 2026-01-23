namespace Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

public sealed record MarketplaceSearchCriteria(
    string? Search,
    string? Category,
    int Page,
    int PageSize);
