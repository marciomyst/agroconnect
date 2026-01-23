using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Domain.Catalog.SellerProducts;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfSellerProductRepository(AgronomiaDbContext context) : ISellerProductRepository
{
    public Task<bool> ExistsAsync(Guid sellerId, Guid productId, CancellationToken ct)
    {
        return context.SellerProducts
            .AsNoTracking()
            .AnyAsync(
                sellerProduct => sellerProduct.SellerId == sellerId && sellerProduct.ProductId == productId,
                ct);
    }

    public Task<SellerProduct?> GetByIdAsync(Guid sellerProductId, CancellationToken ct)
    {
        return context.SellerProducts
            .FirstOrDefaultAsync(sellerProduct => sellerProduct.Id == sellerProductId, ct);
    }

    public async Task AddAsync(SellerProduct sellerProduct, CancellationToken ct)
    {
        await context.SellerProducts.AddAsync(sellerProduct, ct);
    }
}
