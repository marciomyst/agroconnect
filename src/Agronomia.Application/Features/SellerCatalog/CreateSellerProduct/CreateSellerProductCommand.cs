using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.SellerCatalog.CreateSellerProduct;

public sealed record CreateSellerProductCommand(
    Guid SellerId,
    Guid ExecutorUserId,
    Guid ProductId,
    decimal Price,
    string Currency,
    bool IsAvailable
) : ICommand<CreateSellerProductResult>;
