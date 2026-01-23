namespace Agronomia.Application.Features.PurchaseIntents;

public sealed class PurchaseIntentNotFoundException(Guid purchaseIntentId)
    : Exception($"Purchase intent '{purchaseIntentId}' not found.")
{
    public Guid PurchaseIntentId { get; } = purchaseIntentId;
}
