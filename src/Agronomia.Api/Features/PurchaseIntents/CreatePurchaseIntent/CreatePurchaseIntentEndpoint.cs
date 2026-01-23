using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.PurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.CreatePurchaseIntent;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.PurchaseIntents.CreatePurchaseIntent;

public static class CreatePurchaseIntentEndpoint
{
    public static void MapCreatePurchaseIntent(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/purchase-intents", async (
            [FromBody] CreatePurchaseIntentHttpRequest request,
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

            var command = request.ToCommand(farmId, userId);

            try
            {
                var result = await messageBus.InvokeAsync<CreatePurchaseIntentResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/purchase-intents/{response.PurchaseIntentId}", response);
            }
            catch (PurchaseIntentForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot create purchase intents.");
            }
            catch (SellerProductNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Seller product not found.");
            }
            catch (SellerProductNotAvailableException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Seller product is not available.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("CreatePurchaseIntent")
        .WithTags("PurchaseIntents")
        .WithSummary("Create purchase intent.")
        .WithDescription("Creates a purchase intent for the active farm.")
        .RequireAuthorization("User")
        .Produces<CreatePurchaseIntentHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
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
