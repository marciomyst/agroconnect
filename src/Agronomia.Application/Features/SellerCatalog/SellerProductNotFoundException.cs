namespace Agronomia.Application.Features.SellerCatalog;

public sealed class SellerProductNotFoundException(Guid sellerProductId)
    : Exception($"Seller product '{sellerProductId}' not found.")
{
    public Guid SellerProductId { get; } = sellerProductId;
}
