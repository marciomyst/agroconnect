namespace Agronomia.Application.Features.Products.SearchProducts;

public sealed record ProductListItemDto(
    Guid ProductId,
    string Name,
    string Category,
    string UnitOfMeasure,
    string? RegistrationNumber,
    bool IsControlledByRecipe,
    bool IsActive);
