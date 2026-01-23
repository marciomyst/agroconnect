namespace Agronomia.Application.Features.PurchaseIntents;

public sealed class PurchaseIntentStatusTransitionException(string message)
    : Exception(message)
{
}
