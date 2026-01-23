namespace Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;

public sealed record SellerPurchaseIntentDto(
    Guid PurchaseIntentId,
    Guid ProductId,
    string ProductName,
    Guid FarmId,
    string FarmName,
    Guid SellerProductId,
    decimal Quantity,
    string Status,
    DateTime RequestedAtUtc);
