using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

namespace Agronomia.Application.Abstractions.Catalog;

public interface ISellerCatalogReadRepository
{
    Task<PagedResult<SellerCatalogItemDto>> GetSellerCatalogAsync(SellerCatalogCriteria criteria, CancellationToken ct);
}
