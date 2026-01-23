namespace Agronomia.Application.Features.PurchaseIntents;

public sealed class InvalidPurchaseIntentStatusException(string status)
    : Exception($"Purchase intent status '{status}' is invalid.")
{
    public string Status { get; } = status;
}
