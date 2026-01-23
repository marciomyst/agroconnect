using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Catalog.Products;

namespace Agronomia.Application.Features.Products.CreateProduct;

public sealed class CreateProductHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateProductResult> HandleAsync(CreateProductCommand command, CancellationToken ct)
    {
        var normalizedName = command.Name.Trim();
        var normalizedRegistration = string.IsNullOrWhiteSpace(command.RegistrationNumber)
            ? null
            : command.RegistrationNumber.Trim();

        if (await _productRepository.ExistsByNameAndRegistrationNumberAsync(
                normalizedName,
                normalizedRegistration,
                ct))
        {
            throw new ProductAlreadyExistsException(normalizedName, normalizedRegistration);
        }

        var category = Enum.Parse<ProductCategory>(command.Category, ignoreCase: true);
        var unitOfMeasure = Enum.Parse<UnitOfMeasure>(command.UnitOfMeasure, ignoreCase: true);
        var name = ProductName.Create(normalizedName);
        var registrationNumber = RegistrationNumber.CreateOptional(normalizedRegistration);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var product = Product.Create(
                name,
                category,
                unitOfMeasure,
                registrationNumber,
                command.IsControlledByRecipe);

            await _productRepository.AddAsync(product, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreateProductResult(product.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
