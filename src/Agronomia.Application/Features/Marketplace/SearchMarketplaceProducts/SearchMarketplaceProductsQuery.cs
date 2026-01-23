using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

public sealed record SearchMarketplaceProductsQuery(
    Guid FarmId,
    Guid ExecutorUserId,
    string? Search,
    string? Category,
    int Page = 1,
    int PageSize = 20)
    : IQuery<PagedResult<MarketplaceProductListItemDto>>;
