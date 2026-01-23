using Agronomia.Application.Features.SellerCatalog.CreateSellerProduct;

namespace Agronomia.Api.Features.Sellers.Catalog.CreateSellerProduct;

public static class CreateSellerProductMapper
{
    public static CreateSellerProductCommand ToCommand(
        this CreateSellerProductHttpRequest request,
        Guid sellerId,
        Guid executorUserId)
    {
        return new CreateSellerProductCommand(
            sellerId,
            executorUserId,
            request.ProductId,
            request.Price,
            request.Currency,
            request.IsAvailable);
    }

    public static CreateSellerProductHttpResponse FromResult(this CreateSellerProductResult result)
        => new(result.SellerProductId);
}
