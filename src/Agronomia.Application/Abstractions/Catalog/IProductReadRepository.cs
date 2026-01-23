using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Products.GetProductById;
using Agronomia.Application.Features.Products.SearchProducts;

namespace Agronomia.Application.Abstractions.Catalog;

public interface IProductReadRepository
{
    Task<ProductDetailsDto?> GetByIdAsync(Guid productId, CancellationToken ct);

    Task<PagedResult<ProductListItemDto>> SearchAsync(ProductSearchCriteria criteria, CancellationToken ct);
}
