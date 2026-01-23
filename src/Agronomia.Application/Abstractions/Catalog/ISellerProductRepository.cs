using Agronomia.Domain.Catalog.SellerProducts;

namespace Agronomia.Application.Abstractions.Catalog;

public interface ISellerProductRepository
{
    Task<bool> ExistsAsync(Guid sellerId, Guid productId, CancellationToken ct);

    Task<SellerProduct?> GetByIdAsync(Guid sellerProductId, CancellationToken ct);

    Task AddAsync(SellerProduct sellerProduct, CancellationToken ct);
}
