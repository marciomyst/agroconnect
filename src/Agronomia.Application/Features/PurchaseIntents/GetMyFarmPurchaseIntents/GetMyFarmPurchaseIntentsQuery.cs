using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;

public sealed record GetMyFarmPurchaseIntentsQuery(
    Guid FarmId,
    Guid ExecutorUserId)
    : IQuery<IReadOnlyList<FarmPurchaseIntentDto>>;
