namespace Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

public sealed record SellerCatalogItemDto(
    Guid SellerProductId,
    Guid ProductId,
    string ProductName,
    string Category,
    string UnitOfMeasure,
    decimal Price,
    string Currency,
    bool IsAvailable,
    bool IsControlledByRecipe);
