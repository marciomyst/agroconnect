using Agronomia.Application.Features.Authentication.Refresh;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Wolverine;

namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Endpoint for refreshing JWT tokens.
/// </summary>
public static class RefreshTokenEndpoint
{
    /// <summary>
    /// Maps POST /api/auth/refresh.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapRefreshToken(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (
            [FromBody] RefreshTokenRequest request,
            IMessageBus messageBus,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await messageBus.InvokeAsync<RefreshTokenResult?>(command, cancellationToken);

            if (result is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Refresh token is invalid or expired.");
            }

            var response = result.FromResult();
            return Results.Ok(response);
        })
        .WithName("RefreshToken")
        .WithTags("Auth")
        .WithSummary("Refresh JWT access token.")
        .WithDescription("Exchanges a refresh token for a new access token and refresh token pair.")
        .AllowAnonymous()
        .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithMetadata(new SwaggerRequestExampleAttribute(typeof(RefreshTokenRequest), typeof(RefreshTokenRequestExample)))
        .WithMetadata(new SwaggerResponseExampleAttribute(StatusCodes.Status200OK, typeof(RefreshTokenResponseExample)));
    }
}
