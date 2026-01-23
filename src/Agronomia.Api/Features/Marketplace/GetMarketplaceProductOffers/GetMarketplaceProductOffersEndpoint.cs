using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.Marketplace;
using Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Marketplace.GetMarketplaceProductOffers;

public static class GetMarketplaceProductOffersEndpoint
{
    public static void MapGetMarketplaceProductOffers(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/marketplace/products/{productId:guid}/offers", async (
            [FromRoute] Guid productId,
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

            var query = new GetMarketplaceProductOffersQuery(farmId, userId, productId);

            try
            {
                var result = await messageBus.InvokeAsync<IReadOnlyList<MarketplaceProductOfferItemDto>>(query, ct);
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
        .WithName("GetMarketplaceProductOffers")
        .WithTags("Marketplace")
        .WithSummary("Get marketplace offers for a product.")
        .WithDescription("Lists active seller offers for a marketplace product.")
        .RequireAuthorization("User")
        .Produces<IReadOnlyList<MarketplaceProductOfferItemDto>>(StatusCodes.Status200OK)
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
