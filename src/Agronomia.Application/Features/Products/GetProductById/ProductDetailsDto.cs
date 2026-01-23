namespace Agronomia.Application.Features.Products.GetProductById;

public sealed record ProductDetailsDto(
    Guid ProductId,
    string Name,
    string Category,
    string UnitOfMeasure,
    string? RegistrationNumber,
    bool IsControlledByRecipe,
    bool IsActive,
    DateTime CreatedAtUtc);
