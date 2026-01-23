using Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;

namespace Agronomia.Application.Abstractions.Orders;

public interface IPurchaseIntentReadRepository
{
    Task<IReadOnlyList<FarmPurchaseIntentDto>> GetFarmPurchaseIntentsAsync(Guid farmId, CancellationToken ct);

    Task<IReadOnlyList<SellerPurchaseIntentDto>> GetSellerPurchaseIntentsAsync(Guid sellerId, CancellationToken ct);
}
