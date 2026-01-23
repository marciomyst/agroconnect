using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

public sealed record UpdatePurchaseIntentStatusCommand(
    Guid SellerId,
    Guid ExecutorUserId,
    Guid PurchaseIntentId,
    string Status
) : ICommand<UpdatePurchaseIntentStatusResult>;
