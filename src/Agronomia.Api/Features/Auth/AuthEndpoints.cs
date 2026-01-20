using Agronomia.Api.Features.Auth.Authenticate;

namespace Agronomia.Api.Features.Auth;

/// <summary>
/// Maps all authentication-related endpoints.
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// Maps all auth endpoints.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthenticateUser();
    }
}
