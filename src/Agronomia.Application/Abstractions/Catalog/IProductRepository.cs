using Agronomia.Domain.Catalog.Products;

namespace Agronomia.Application.Abstractions.Catalog;

public interface IProductRepository
{
    Task<bool> ExistsByNameAndRegistrationNumberAsync(
        string name,
        string? registrationNumber,
        CancellationToken ct);

    Task<bool> ExistsAsync(Guid productId, CancellationToken ct);

    Task AddAsync(Product product, CancellationToken ct);
}
