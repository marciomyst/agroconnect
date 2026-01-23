using Agronomia.Application.Features.SellerCatalog.UpdateSellerProduct;

namespace Agronomia.Api.Features.Sellers.Catalog.UpdateSellerProduct;

public static class UpdateSellerProductMapper
{
    public static UpdateSellerProductCommand ToCommand(
        this UpdateSellerProductHttpRequest request,
        Guid sellerId,
        Guid sellerProductId,
        Guid executorUserId)
    {
        return new UpdateSellerProductCommand(
            sellerId,
            executorUserId,
            sellerProductId,
            request.Price,
            request.Currency,
            request.IsAvailable);
    }

    public static UpdateSellerProductHttpResponse FromResult(this UpdateSellerProductResult result)
        => new(result.SellerProductId);
}
