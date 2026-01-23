using System.Data;
using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;
using Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;
using Dapper;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

public sealed class MarketplaceReadRepository(IDbConnection connection) : IMarketplaceReadRepository
{
    private const string CountSql = """
        SELECT COUNT(DISTINCT p."Id")
        FROM products p
        INNER JOIN seller_products sp ON sp."ProductId" = p."Id"
        WHERE p."IsActive" = TRUE
          AND sp."IsAvailable" = TRUE
          AND (@Search IS NULL OR p."Name" ILIKE @Search)
          AND (@Category IS NULL OR p."Category" = @Category);
        """;

    private const string SearchSql = """
        SELECT
            p."Id"                 AS "ProductId",
            p."Name"               AS "Name",
            p."Category"           AS "Category",
            p."IsControlledByRecipe" AS "IsControlledByRecipe",
            MIN(sp."Price")        AS "BestPrice",
            TRUE                   AS "HasAvailableOffers"
        FROM products p
        INNER JOIN seller_products sp ON sp."ProductId" = p."Id"
        WHERE p."IsActive" = TRUE
          AND sp."IsAvailable" = TRUE
          AND (@Search IS NULL OR p."Name" ILIKE @Search)
          AND (@Category IS NULL OR p."Category" = @Category)
        GROUP BY p."Id"
        ORDER BY p."Name"
        LIMIT @PageSize OFFSET @Offset;
        """;

    private const string OffersSql = """
        SELECT
            sp."Id"             AS "SellerProductId",
            sp."SellerId"       AS "SellerId",
            s."CorporateName"   AS "SellerName",
            sp."Price"          AS "Price",
            sp."Currency"       AS "Currency",
            sp."IsAvailable"    AS "IsAvailable"
        FROM seller_products sp
        INNER JOIN sellers s ON s."Id" = sp."SellerId"
        WHERE sp."ProductId" = @ProductId
          AND sp."IsAvailable" = TRUE
        ORDER BY sp."Price";
        """;

    public async Task<PagedResult<MarketplaceProductListItemDto>> SearchProductsAsync(
        MarketplaceSearchCriteria criteria,
        CancellationToken ct)
    {
        var search = string.IsNullOrWhiteSpace(criteria.Search)
            ? null
            : $"%{criteria.Search.Trim()}%";

        var parameters = new
        {
            Search = search,
            Category = string.IsNullOrWhiteSpace(criteria.Category) ? null : criteria.Category,
            PageSize = criteria.PageSize,
            Offset = (criteria.Page - 1) * criteria.PageSize
        };

        var countCommand = new CommandDefinition(CountSql, parameters, cancellationToken: ct);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);

        var itemsCommand = new CommandDefinition(SearchSql, parameters, cancellationToken: ct);
        var items = (await connection.QueryAsync<MarketplaceProductListItemDto>(itemsCommand)).ToList();

        return new PagedResult<MarketplaceProductListItemDto>(items, criteria.Page, criteria.PageSize, totalCount);
    }

    public async Task<IReadOnlyList<MarketplaceProductOfferItemDto>> GetProductOffersAsync(
        Guid productId,
        CancellationToken ct)
    {
        var command = new CommandDefinition(
            OffersSql,
            new { ProductId = productId },
            cancellationToken: ct);

        var items = await connection.QueryAsync<MarketplaceProductOfferItemDto>(command);
        return items.ToList();
    }
}
