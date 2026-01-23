using Agronomia.Application.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

namespace Agronomia.Api.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

public static class UpdatePurchaseIntentStatusMapper
{
    public static UpdatePurchaseIntentStatusCommand ToCommand(
        this UpdatePurchaseIntentStatusHttpRequest request,
        Guid sellerId,
        Guid executorUserId,
        Guid purchaseIntentId)
    {
        return new UpdatePurchaseIntentStatusCommand(
            sellerId,
            executorUserId,
            purchaseIntentId,
            request.Status);
    }

    public static UpdatePurchaseIntentStatusHttpResponse FromResult(this UpdatePurchaseIntentStatusResult result)
        => new(result.PurchaseIntentId);
}
