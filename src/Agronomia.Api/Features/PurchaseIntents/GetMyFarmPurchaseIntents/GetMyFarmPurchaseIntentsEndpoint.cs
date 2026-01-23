using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.PurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;
using FluentValidation;
using Wolverine;

namespace Agronomia.Api.Features.PurchaseIntents.GetMyFarmPurchaseIntents;

public static class GetMyFarmPurchaseIntentsEndpoint
{
    public static void MapGetMyFarmPurchaseIntents(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/me/purchase-intents", async (
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

            var query = new GetMyFarmPurchaseIntentsQuery(farmId, userId);

            try
            {
                var result = await messageBus.InvokeAsync<IReadOnlyList<FarmPurchaseIntentDto>>(query, ct);
                return Results.Ok(result);
            }
            catch (PurchaseIntentForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot access purchase intents.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("GetMyFarmPurchaseIntents")
        .WithTags("PurchaseIntents")
        .WithSummary("List purchase intents for the active farm.")
        .WithDescription("Lists purchase intents created by the active farm.")
        .RequireAuthorization("User")
        .Produces<IReadOnlyList<FarmPurchaseIntentDto>>(StatusCodes.Status200OK)
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
