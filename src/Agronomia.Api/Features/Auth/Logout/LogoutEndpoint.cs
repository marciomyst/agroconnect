using Agronomia.Application.Features.Authentication.Logout;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Wolverine;

namespace Agronomia.Api.Features.Auth.Logout;

/// <summary>
/// Endpoint for revoking refresh tokens.
/// </summary>
public static class LogoutEndpoint
{
    /// <summary>
    /// Maps POST /api/auth/logout.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapLogout(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (
            [FromBody] LogoutRequest request,
            IMessageBus messageBus,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await messageBus.InvokeAsync<LogoutResult?>(command, cancellationToken);

            if (result is null || result.Revoked is false)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Refresh token is invalid or already revoked.");
            }

            return Results.NoContent();
        })
        .WithName("Logout")
        .WithTags("Auth")
        .WithSummary("Revoke refresh token.")
        .WithDescription("Logs out the user by revoking the provided refresh token and preventing further refresh operations.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithMetadata(new SwaggerRequestExampleAttribute(typeof(LogoutRequest), typeof(LogoutRequestExample)));
    }
}
