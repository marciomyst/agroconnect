namespace Agronomia.Api.Features.Sellers.Catalog.CreateSellerProduct;

public sealed record CreateSellerProductHttpRequest(
    Guid ProductId,
    decimal Price,
    string Currency,
    bool IsAvailable);
