namespace Agronomia.Api.Features.Products.CreateProduct;

public sealed record CreateProductHttpRequest(
    string Name,
    string Category,
    string UnitOfMeasure,
    string? RegistrationNumber,
    bool IsControlledByRecipe);
