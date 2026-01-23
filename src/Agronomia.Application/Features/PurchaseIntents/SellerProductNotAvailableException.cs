namespace Agronomia.Application.Features.PurchaseIntents;

public sealed class SellerProductNotAvailableException(Guid sellerProductId)
    : Exception($"Seller product '{sellerProductId}' is not available.")
{
    public Guid SellerProductId { get; } = sellerProductId;
}
