namespace Agronomia.Application.Features.PurchaseIntents;

public sealed class PurchaseIntentForbiddenException()
    : Exception("User does not have permission to manage purchase intents.")
{
}
