using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

public sealed record GetSellerCatalogQuery(
    Guid SellerId,
    Guid ExecutorUserId,
    string? Search,
    int Page = 1,
    int PageSize = 20)
    : IQuery<PagedResult<SellerCatalogItemDto>>;
