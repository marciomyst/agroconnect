namespace Agronomia.Application.Features.SellerCatalog;

public sealed class ProductNotFoundException(Guid productId)
    : Exception($"Product '{productId}' not found.")
{
    public Guid ProductId { get; } = productId;
}
