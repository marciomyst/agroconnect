using Agronomia.Domain.Organizations;

namespace Agronomia.Application.Abstractions.Organizations;

public interface IFarmRepository
{
    Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct);

    Task<bool> ExistsAsync(Guid farmId, CancellationToken ct);

    Task AddAsync(Farm farm, CancellationToken ct);
}
