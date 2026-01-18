using System.Security.Claims;
using Agronomia.Application.Features.Users;
using Agronomia.Application.Features.Users.GetCurrentUser;
using Agronomia.Domain.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Wolverine;

namespace Agronomia.Api.Features.Users.GetCurrentUser;

/// <summary>
/// Maps the authenticated user endpoint (/api/users/me).
/// </summary>
public static class GetCurrentUserEndpoint
{
    /// <summary>
    /// Maps GET /api/users/me to retrieve the current authenticated user.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapGetCurrentUser(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me", async (
            ClaimsPrincipal principal,
            IMessageBus messageBus,
            ICacheService cache,
            CancellationToken cancellationToken) =>
        {
            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                         ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Results.Unauthorized();
            }

            var cacheKey = $"user:me:{userId}";
            var cached = await cache.GetAsync<UserDto>(cacheKey);
            if (cached is not null)
            {
                return Results.Ok(cached);
            }

            var query = new GetCurrentUserQuery(userId);
            var user = await messageBus.InvokeAsync<UserDto?>(query, cancellationToken);

            if (user is null)
            {
                return Results.NotFound();
            }

            await cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(5));

            return Results.Ok(user);
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithTags("Users")
        .WithSummary("Obtem o usuario autenticado.")
        .WithDescription("Retorna os dados basicos do usuario autenticado para montar o dropdown.");
    }
}
