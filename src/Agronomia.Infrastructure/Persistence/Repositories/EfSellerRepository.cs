using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfSellerRepository(AgronomiaDbContext context) : ISellerRepository
{
    public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
    {
        return context.Sellers
            .AsNoTracking()
            .AnyAsync(seller => seller.TaxId == taxId, ct);
    }

    public Task<bool> ExistsAsync(Guid sellerId, CancellationToken ct)
    {
        return context.Sellers
            .AsNoTracking()
            .AnyAsync(seller => seller.Id == sellerId, ct);
    }

    public async Task AddAsync(Seller seller, CancellationToken ct)
    {
        await context.Sellers.AddAsync(seller, ct);
    }
}
