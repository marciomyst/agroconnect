using Agronomia.Application.Features.PurchaseIntents.CreatePurchaseIntent;

namespace Agronomia.Api.Features.PurchaseIntents.CreatePurchaseIntent;

public static class CreatePurchaseIntentMapper
{
    public static CreatePurchaseIntentCommand ToCommand(
        this CreatePurchaseIntentHttpRequest request,
        Guid farmId,
        Guid executorUserId)
    {
        return new CreatePurchaseIntentCommand(
            farmId,
            executorUserId,
            request.SellerProductId,
            request.Quantity,
            request.Notes);
    }

    public static CreatePurchaseIntentHttpResponse FromResult(this CreatePurchaseIntentResult result)
        => new(result.PurchaseIntentId);
}
