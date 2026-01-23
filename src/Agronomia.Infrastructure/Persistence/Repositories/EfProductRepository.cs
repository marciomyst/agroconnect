using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Domain.Catalog.Products;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfProductRepository(AgronomiaDbContext context) : IProductRepository
{
    public Task<bool> ExistsByNameAndRegistrationNumberAsync(
        string name,
        string? registrationNumber,
        CancellationToken ct)
    {
        var productName = ProductName.Create(name);
        var registration = RegistrationNumber.CreateOptional(registrationNumber);

        return context.Products
            .AsNoTracking()
            .AnyAsync(
                product => product.Name == productName && product.RegistrationNumber == registration,
                ct);
    }

    public Task<bool> ExistsAsync(Guid productId, CancellationToken ct)
    {
        return context.Products
            .AsNoTracking()
            .AnyAsync(product => product.Id == productId, ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct)
    {
        await context.Products.AddAsync(product, ct);
    }
}
