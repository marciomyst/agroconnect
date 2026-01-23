using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;

public sealed class GetMyFarmPurchaseIntentsHandler(
    IFarmMembershipRepository farmMembershipRepository,
    IPurchaseIntentRepository purchaseIntentRepository,
    IPurchaseIntentReadRepository purchaseIntentReadRepository)
    : IQueryHandler<GetMyFarmPurchaseIntentsQuery, IReadOnlyList<FarmPurchaseIntentDto>>
{
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;
    private readonly IPurchaseIntentRepository _purchaseIntentRepository = purchaseIntentRepository;
    private readonly IPurchaseIntentReadRepository _purchaseIntentReadRepository = purchaseIntentReadRepository;

    public async Task<IReadOnlyList<FarmPurchaseIntentDto>> HandleAsync(
        GetMyFarmPurchaseIntentsQuery query,
        CancellationToken ct)
    {
        var memberships = await _farmMembershipRepository.GetByFarmAndUserAsync(
            query.FarmId,
            query.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is FarmRole.Owner or FarmRole.Buyer))
        {
            throw new PurchaseIntentForbiddenException();
        }

        var cutoffUtc = DateTime.UtcNow.AddDays(-30);
        await _purchaseIntentRepository.ExpirePendingAsync(cutoffUtc, ct);

        return await _purchaseIntentReadRepository.GetFarmPurchaseIntentsAsync(query.FarmId, ct);
    }
}
