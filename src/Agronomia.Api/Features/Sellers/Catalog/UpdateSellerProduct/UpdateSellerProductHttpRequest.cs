namespace Agronomia.Api.Features.Sellers.Catalog.UpdateSellerProduct;

public sealed record UpdateSellerProductHttpRequest(
    decimal Price,
    string Currency,
    bool IsAvailable);
