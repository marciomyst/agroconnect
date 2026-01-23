using Agronomia.Application.Features.Products.CreateProduct;

namespace Agronomia.Api.Features.Products.CreateProduct;

public static class CreateProductMapper
{
    public static CreateProductCommand ToCommand(this CreateProductHttpRequest request)
    {
        return new CreateProductCommand(
            request.Name,
            request.Category,
            request.UnitOfMeasure,
            request.RegistrationNumber,
            request.IsControlledByRecipe);
    }

    public static CreateProductHttpResponse FromResult(this CreateProductResult result)
        => new(result.ProductId);
}
