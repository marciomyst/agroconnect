namespace Agronomia.Application.Features.SellerCatalog;

public sealed class SellerNotFoundException(Guid sellerId)
    : Exception($"Seller '{sellerId}' not found.")
{
    public Guid SellerId { get; } = sellerId;
}
