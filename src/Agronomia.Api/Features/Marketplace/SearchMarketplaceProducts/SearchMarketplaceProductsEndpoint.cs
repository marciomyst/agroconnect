using Agronomia.Api.Features.Shared;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.Marketplace;
using Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Marketplace.SearchMarketplaceProducts;

public static class SearchMarketplaceProductsEndpoint
{
    public static void MapSearchMarketplaceProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/marketplace/products", async (
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            HttpContext httpContext,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            if (!RequestContext.TryGetUserId(httpContext, out var userId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid token.");
            }

            if (!RequestContext.TryGetOrganizationId(httpContext, out var farmId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request",
                    detail: "Active farm organization is required.");
            }

            var query = new SearchMarketplaceProductsQuery(
                farmId,
                userId,
                search,
                category,
                page <= 0 ? 1 : page,
                pageSize <= 0 ? 20 : pageSize);

            try
            {
                var result = await messageBus.InvokeAsync<PagedResult<MarketplaceProductListItemDto>>(query, ct);
                return Results.Ok(result);
            }
            catch (MarketplaceForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot access the marketplace.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("SearchMarketplaceProducts")
        .WithTags("Marketplace")
        .WithSummary("Search marketplace products.")
        .WithDescription("Lists marketplace products with active offers.")
        .RequireAuthorization("User")
        .Produces<PagedResult<MarketplaceProductListItemDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
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
