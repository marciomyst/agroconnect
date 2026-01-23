using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;

public sealed class GetMySellerPurchaseIntentsHandler(
    ISellerMembershipRepository sellerMembershipRepository,
    IPurchaseIntentRepository purchaseIntentRepository,
    IPurchaseIntentReadRepository purchaseIntentReadRepository)
    : IQueryHandler<GetMySellerPurchaseIntentsQuery, IReadOnlyList<SellerPurchaseIntentDto>>
{
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IPurchaseIntentRepository _purchaseIntentRepository = purchaseIntentRepository;
    private readonly IPurchaseIntentReadRepository _purchaseIntentReadRepository = purchaseIntentReadRepository;

    public async Task<IReadOnlyList<SellerPurchaseIntentDto>> HandleAsync(
        GetMySellerPurchaseIntentsQuery query,
        CancellationToken ct)
    {
        var memberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            query.SellerId,
            query.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is SellerRole.Owner or SellerRole.Manager))
        {
            throw new PurchaseIntentForbiddenException();
        }

        var cutoffUtc = DateTime.UtcNow.AddDays(-30);
        await _purchaseIntentRepository.ExpirePendingAsync(cutoffUtc, ct);

        return await _purchaseIntentReadRepository.GetSellerPurchaseIntentsAsync(query.SellerId, ct);
    }
}
