using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.PurchaseIntents.CreatePurchaseIntent;

public sealed record CreatePurchaseIntentCommand(
    Guid FarmId,
    Guid ExecutorUserId,
    Guid SellerProductId,
    decimal Quantity,
    string? Notes
) : ICommand<CreatePurchaseIntentResult>;
