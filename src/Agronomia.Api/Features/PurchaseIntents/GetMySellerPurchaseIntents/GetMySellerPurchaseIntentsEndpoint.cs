using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.PurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;
using FluentValidation;
using Wolverine;

namespace Agronomia.Api.Features.PurchaseIntents.GetMySellerPurchaseIntents;

public static class GetMySellerPurchaseIntentsEndpoint
{
    public static void MapGetMySellerPurchaseIntents(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/me/seller/purchase-intents", async (
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

            if (!RequestContext.TryGetOrganizationId(httpContext, out var sellerId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request",
                    detail: "Active seller organization is required.");
            }

            var query = new GetMySellerPurchaseIntentsQuery(sellerId, userId);

            try
            {
                var result = await messageBus.InvokeAsync<IReadOnlyList<SellerPurchaseIntentDto>>(query, ct);
                return Results.Ok(result);
            }
            catch (PurchaseIntentForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot access seller purchase intents.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("GetMySellerPurchaseIntents")
        .WithTags("PurchaseIntents")
        .WithSummary("List purchase intents for the active seller.")
        .WithDescription("Lists purchase intents received by the active seller.")
        .RequireAuthorization("User")
        .Produces<IReadOnlyList<SellerPurchaseIntentDto>>(StatusCodes.Status200OK)
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
