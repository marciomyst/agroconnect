using Agronomia.Api.Features.Users.GetCurrentUser;

namespace Agronomia.Api.Features.Users;

/// <summary>
/// Maps user-related endpoints.
/// </summary>
public static class UserEndpoints
{
    /// <summary>
    /// Maps all user endpoints.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetCurrentUser();
    }
}