using Agronomia.Application.Features.Identity.GetCurrentUserContext;
using Wolverine;

namespace Agronomia.Api.Features.Auth.GetCurrentUserContext;

public static class GetCurrentUserContextEndpoint
{
    public static void MapGetCurrentUserContext(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", async (
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            try
            {
                var result = await messageBus.InvokeAsync<CurrentUserContextResponse>(
                    new GetCurrentUserContextQuery(),
                    ct);
                return Results.Ok(result);
            }
            catch (CurrentUserNotAuthenticatedException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid token.");
            }
            catch (CurrentUserNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "User not found.");
            }
        })
        .WithName("GetCurrentUserContext")
        .WithTags("Auth")
        .WithSummary("Get current user context.")
        .WithDescription("Returns the authenticated user context and memberships.")
        .RequireAuthorization("User")
        .Produces<CurrentUserContextResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
