using Agronomia.Api.Features.Auth.Refresh;
using Agronomia.Application.Features.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Wolverine;

namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Contains endpoint mappings for login functionality in the authentication feature.
/// </summary>
public static class LoginEndpoint
{
    /// <summary>
    /// Maps the POST endpoint for user login, handling authentication and returning a JWT token.
    /// </summary>
    /// <param name="app">The endpoint route builder to extend.</param>
    /// <remarks>
    /// <para>
    /// This endpoint accepts a <see cref="LoginRequest"/> with user credentials and returns a <see cref="RefreshTokenResponse"/>
    /// containing the JWT bearer token for subsequent authenticated requests.
    /// </para>
    /// <para>
    /// The endpoint is configured with Swagger metadata and produces HTTP 200 OK status on success.
    /// </para>
    /// </remarks>
    /// <example>
    /// POST /api/auth/login
    /// {
    ///   "email": "user@example.com",
    ///   "password": "P@ssw0rd!"
    /// }
    /// Response: 200 OK
    /// {
    ///   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    /// }
    /// </example>
    public static void MapLogin(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            [FromBody] LoginRequest request,
            IMessageBus messageBus,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await messageBus.InvokeAsync<LoginResult?>(command, cancellationToken);

            if (result is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid email or password.");
            }

            var response = result.FromResult();
            return Results.Ok(response);
        })
        .WithName("Login")
        .WithTags("Auth")
        .WithSummary("Authenticate user and return JWT token.")
        .WithDescription("Logs in a user with email and password, returning a JWT bearer token for authenticated requests.")
        .AllowAnonymous()
        .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithMetadata(new SwaggerRequestExampleAttribute(typeof(LoginRequest), typeof(LoginRequestExample)))
        .WithMetadata(new SwaggerResponseExampleAttribute(StatusCodes.Status200OK, typeof(LoginResponseExample)));
    }
}
