using System.Data;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;
using Dapper;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

public sealed class SellerCatalogReadRepository(IDbConnection connection) : ISellerCatalogReadRepository
{
    private const string CountSql = """
        SELECT COUNT(*)
        FROM seller_products sp
        INNER JOIN products p ON p."Id" = sp."ProductId"
        WHERE sp."SellerId" = @SellerId
          AND (@Search IS NULL OR p."Name" ILIKE @Search);
        """;

    private const string CatalogSql = """
        SELECT
            sp."Id"               AS "SellerProductId",
            sp."ProductId"        AS "ProductId",
            p."Name"              AS "ProductName",
            p."Category"          AS "Category",
            p."UnitOfMeasure"     AS "UnitOfMeasure",
            sp."Price"            AS "Price",
            sp."Currency"         AS "Currency",
            sp."IsAvailable"      AS "IsAvailable",
            p."IsControlledByRecipe" AS "IsControlledByRecipe"
        FROM seller_products sp
        INNER JOIN products p ON p."Id" = sp."ProductId"
        WHERE sp."SellerId" = @SellerId
          AND (@Search IS NULL OR p."Name" ILIKE @Search)
        ORDER BY p."Name"
        LIMIT @PageSize OFFSET @Offset;
        """;

    public async Task<PagedResult<SellerCatalogItemDto>> GetSellerCatalogAsync(
        SellerCatalogCriteria criteria,
        CancellationToken ct)
    {
        var search = string.IsNullOrWhiteSpace(criteria.Search)
            ? null
            : $"%{criteria.Search.Trim()}%";

        var parameters = new
        {
            SellerId = criteria.SellerId,
            Search = search,
            PageSize = criteria.PageSize,
            Offset = (criteria.Page - 1) * criteria.PageSize
        };

        var countCommand = new CommandDefinition(CountSql, parameters, cancellationToken: ct);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);

        var itemsCommand = new CommandDefinition(CatalogSql, parameters, cancellationToken: ct);
        var items = (await connection.QueryAsync<SellerCatalogItemDto>(itemsCommand)).ToList();

        return new PagedResult<SellerCatalogItemDto>(items, criteria.Page, criteria.PageSize, totalCount);
    }
}
