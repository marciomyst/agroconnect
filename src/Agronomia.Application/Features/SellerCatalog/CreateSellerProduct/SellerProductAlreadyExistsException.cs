namespace Agronomia.Application.Features.SellerCatalog.CreateSellerProduct;

public sealed class SellerProductAlreadyExistsException(Guid sellerId, Guid productId)
    : Exception($"Seller product already exists for seller '{sellerId}' and product '{productId}'.")
{
    public Guid SellerId { get; } = sellerId;

    public Guid ProductId { get; } = productId;
}
