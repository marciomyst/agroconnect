namespace Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

public sealed record SellerCatalogCriteria(
    Guid SellerId,
    string? Search,
    int Page,
    int PageSize);
