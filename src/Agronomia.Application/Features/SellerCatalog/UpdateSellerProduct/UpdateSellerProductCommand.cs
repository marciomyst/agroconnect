using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.SellerCatalog.UpdateSellerProduct;

public sealed record UpdateSellerProductCommand(
    Guid SellerId,
    Guid ExecutorUserId,
    Guid SellerProductId,
    decimal Price,
    string Currency,
    bool IsAvailable
) : ICommand<UpdateSellerProductResult>;
