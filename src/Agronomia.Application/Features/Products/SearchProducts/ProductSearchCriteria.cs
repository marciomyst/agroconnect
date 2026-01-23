namespace Agronomia.Application.Features.Products.SearchProducts;

public sealed record ProductSearchCriteria(
    string? Search,
    string? Category,
    int Page,
    int PageSize);
