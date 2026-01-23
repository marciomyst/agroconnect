using Agronomia.Api.Features.Shared;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Features.SellerCatalog;
using Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.Catalog.GetSellerCatalog;

public static class GetSellerCatalogEndpoint
{
    public static void MapGetSellerCatalog(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/sellers/{sellerId:guid}/catalog", async (
            [FromRoute] Guid sellerId,
            [FromQuery] string? search,
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

            if (!RequestContext.TryGetOrganizationId(httpContext, out var organizationId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request",
                    detail: "Active organization is required.");
            }

            if (organizationId != sellerId)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "Seller context does not match the active organization.");
            }

            var query = new GetSellerCatalogQuery(
                sellerId,
                userId,
                search,
                page <= 0 ? 1 : page,
                pageSize <= 0 ? 20 : pageSize);

            try
            {
                var result = await messageBus.InvokeAsync<PagedResult<SellerCatalogItemDto>>(query, ct);
                return Results.Ok(result);
            }
            catch (SellerCatalogForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot access this seller catalog.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("GetSellerCatalog")
        .WithTags("SellerCatalog")
        .WithSummary("Get seller catalog.")
        .WithDescription("Lists the seller catalog items.")
        .RequireAuthorization("User")
        .Produces<PagedResult<SellerCatalogItemDto>>(StatusCodes.Status200OK)
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
