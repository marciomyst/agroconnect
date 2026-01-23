using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Products.SearchProducts;

public sealed record SearchProductsQuery(
    string? Search,
    string? Category,
    int Page = 1,
    int PageSize = 20)
    : IQuery<PagedResult<ProductListItemDto>>;
