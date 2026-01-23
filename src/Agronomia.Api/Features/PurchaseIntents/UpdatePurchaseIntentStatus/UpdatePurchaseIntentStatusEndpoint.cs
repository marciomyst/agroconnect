using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.PurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.UpdatePurchaseIntentStatus;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

public static class UpdatePurchaseIntentStatusEndpoint
{
    public static void MapUpdatePurchaseIntentStatus(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/purchase-intents/{purchaseIntentId:guid}/status", async (
            [FromRoute] Guid purchaseIntentId,
            [FromBody] UpdatePurchaseIntentStatusHttpRequest request,
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

            var command = request.ToCommand(sellerId, userId, purchaseIntentId);

            try
            {
                var result = await messageBus.InvokeAsync<UpdatePurchaseIntentStatusResult>(command, ct);
                var response = result.FromResult();
                return Results.Ok(response);
            }
            catch (PurchaseIntentForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot update purchase intents.");
            }
            catch (PurchaseIntentNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Purchase intent not found.");
            }
            catch (InvalidPurchaseIntentStatusException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request",
                    detail: "Status is invalid.");
            }
            catch (PurchaseIntentStatusTransitionException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Status transition is not allowed.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("UpdatePurchaseIntentStatus")
        .WithTags("PurchaseIntents")
        .WithSummary("Update purchase intent status.")
        .WithDescription("Updates the status of a purchase intent for the active seller.")
        .RequireAuthorization("User")
        .Produces<UpdatePurchaseIntentStatusHttpResponse>(StatusCodes.Status200OK)
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
