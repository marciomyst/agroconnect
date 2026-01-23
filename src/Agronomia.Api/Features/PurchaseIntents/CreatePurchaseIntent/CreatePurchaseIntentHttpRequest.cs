namespace Agronomia.Api.Features.PurchaseIntents.CreatePurchaseIntent;

public sealed record CreatePurchaseIntentHttpRequest(
    Guid SellerProductId,
    decimal Quantity,
    string? Notes);
