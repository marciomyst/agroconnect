using System.Data;
using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Products.GetProductById;
using Agronomia.Application.Features.Products.SearchProducts;
using Dapper;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

public sealed class ProductReadRepository(IDbConnection connection) : IProductReadRepository
{
    private const string GetByIdSql = """
        SELECT
            p."Id"                 AS "ProductId",
            p."Name"               AS "Name",
            p."Category"           AS "Category",
            p."UnitOfMeasure"      AS "UnitOfMeasure",
            p."RegistrationNumber" AS "RegistrationNumber",
            p."IsControlledByRecipe" AS "IsControlledByRecipe",
            p."IsActive"           AS "IsActive",
            p."CreatedAtUtc"       AS "CreatedAtUtc"
        FROM products p
        WHERE p."Id" = @ProductId
        LIMIT 1;
        """;

    private const string CountSql = """
        SELECT COUNT(*)
        FROM products p
        WHERE (@Search IS NULL OR p."Name" ILIKE @Search)
          AND (@Category IS NULL OR p."Category" = @Category);
        """;

    private const string SearchSql = """
        SELECT
            p."Id"                 AS "ProductId",
            p."Name"               AS "Name",
            p."Category"           AS "Category",
            p."UnitOfMeasure"      AS "UnitOfMeasure",
            p."RegistrationNumber" AS "RegistrationNumber",
            p."IsControlledByRecipe" AS "IsControlledByRecipe",
            p."IsActive"           AS "IsActive"
        FROM products p
        WHERE (@Search IS NULL OR p."Name" ILIKE @Search)
          AND (@Category IS NULL OR p."Category" = @Category)
        ORDER BY p."Name"
        LIMIT @PageSize OFFSET @Offset;
        """;

    public async Task<ProductDetailsDto?> GetByIdAsync(Guid productId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            GetByIdSql,
            new { ProductId = productId },
            cancellationToken: ct);

        return await connection.QuerySingleOrDefaultAsync<ProductDetailsDto>(command);
    }

    public async Task<PagedResult<ProductListItemDto>> SearchAsync(
        ProductSearchCriteria criteria,
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
        var items = (await connection.QueryAsync<ProductListItemDto>(itemsCommand)).ToList();

        return new PagedResult<ProductListItemDto>(items, criteria.Page, criteria.PageSize, totalCount);
    }
}
