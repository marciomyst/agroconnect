using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;

public sealed record GetMySellerPurchaseIntentsQuery(
    Guid SellerId,
    Guid ExecutorUserId)
    : IQuery<IReadOnlyList<SellerPurchaseIntentDto>>;
