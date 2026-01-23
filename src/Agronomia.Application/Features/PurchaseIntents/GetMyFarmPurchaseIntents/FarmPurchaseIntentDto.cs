namespace Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;

public sealed record FarmPurchaseIntentDto(
    Guid PurchaseIntentId,
    Guid ProductId,
    string ProductName,
    Guid SellerId,
    string SellerName,
    Guid SellerProductId,
    decimal Quantity,
    string Status,
    DateTime RequestedAtUtc);
