using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Products.SearchProducts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Products.SearchProducts;

public static class SearchProductsEndpoint
{
    public static void MapSearchProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var query = new SearchProductsQuery(search, category, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize);

            try
            {
                var result = await messageBus.InvokeAsync<PagedResult<ProductListItemDto>>(query, ct);
                return Results.Ok(result);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("SearchProducts")
        .WithTags("Products")
        .WithSummary("Search products.")
        .WithDescription("Searches products by name and category.")
        .RequireAuthorization("User")
        .Produces<PagedResult<ProductListItemDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private static IDictionary<string, string[]> ToValidationErrors(ValidationException ex)
    {
        return ex.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
