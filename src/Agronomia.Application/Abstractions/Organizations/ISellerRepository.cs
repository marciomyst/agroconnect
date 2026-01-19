using Agronomia.Domain.Organizations;

namespace Agronomia.Application.Abstractions.Organizations;

public interface ISellerRepository
{
    Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct);

    Task AddAsync(Seller seller, CancellationToken ct);
}
