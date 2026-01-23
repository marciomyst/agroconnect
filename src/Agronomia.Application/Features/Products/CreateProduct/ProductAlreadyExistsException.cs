namespace Agronomia.Application.Features.Products.CreateProduct;

public sealed class ProductAlreadyExistsException(string name, string? registrationNumber)
    : Exception($"Product '{name}' with registration '{registrationNumber ?? "none"}' already exists.")
{
    public string Name { get; } = name;

    public string? RegistrationNumber { get; } = registrationNumber;
}
