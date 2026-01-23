using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Products.SearchProducts;

public sealed class SearchProductsHandler(IProductReadRepository readRepository)
    : IQueryHandler<SearchProductsQuery, PagedResult<ProductListItemDto>>
{
    private readonly IProductReadRepository _readRepository = readRepository;

    public Task<PagedResult<ProductListItemDto>> HandleAsync(SearchProductsQuery query, CancellationToken ct)
    {
        var criteria = new ProductSearchCriteria(
            query.Search,
            query.Category,
            query.Page,
            query.PageSize);

        return _readRepository.SearchAsync(criteria, ct);
    }
}
