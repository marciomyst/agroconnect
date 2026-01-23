using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Category,
    string UnitOfMeasure,
    string? RegistrationNumber,
    bool IsControlledByRecipe
) : ICommand<CreateProductResult>;
