using Agronomia.Application.Abstractions.Orders;
using Agronomia.Domain.Orders.PurchaseIntents;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfPurchaseIntentRepository(AgronomiaDbContext context) : IPurchaseIntentRepository
{
    public Task<PurchaseIntent?> GetByIdAsync(Guid purchaseIntentId, CancellationToken ct)
    {
        return context.PurchaseIntents
            .FirstOrDefaultAsync(intent => intent.Id == purchaseIntentId, ct);
    }

    public async Task<int> ExpirePendingAsync(DateTime cutoffUtc, CancellationToken ct)
    {
        return await context.PurchaseIntents
            .Where(intent =>
                intent.Status == PurchaseIntentStatus.Pending &&
                intent.RequestedAtUtc <= cutoffUtc)
            .ExecuteUpdateAsync(
                updates => updates
                    .SetProperty(intent => intent.Status, PurchaseIntentStatus.Expired)
                    .SetProperty(intent => intent.UpdatedAtUtc, DateTime.UtcNow),
                ct);
    }

    public async Task AddAsync(PurchaseIntent purchaseIntent, CancellationToken ct)
    {
        await context.PurchaseIntents.AddAsync(purchaseIntent, ct);
    }
}
