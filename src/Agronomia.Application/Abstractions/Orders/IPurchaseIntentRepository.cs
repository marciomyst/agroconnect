using Agronomia.Domain.Orders.PurchaseIntents;

namespace Agronomia.Application.Abstractions.Orders;

public interface IPurchaseIntentRepository
{
    Task<PurchaseIntent?> GetByIdAsync(Guid purchaseIntentId, CancellationToken ct);

    Task<int> ExpirePendingAsync(DateTime cutoffUtc, CancellationToken ct);

    Task AddAsync(PurchaseIntent purchaseIntent, CancellationToken ct);
}
