using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfFarmRepository(AgronomiaDbContext context) : IFarmRepository
{
    public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
    {
        return context.Farms
            .AsNoTracking()
            .AnyAsync(farm => farm.TaxId == taxId, ct);
    }

    public Task<bool> ExistsAsync(Guid farmId, CancellationToken ct)
    {
        return context.Farms
            .AsNoTracking()
            .AnyAsync(farm => farm.Id == farmId, ct);
    }

    public async Task AddAsync(Farm farm, CancellationToken ct)
    {
        await context.Farms.AddAsync(farm, ct);
    }
}
