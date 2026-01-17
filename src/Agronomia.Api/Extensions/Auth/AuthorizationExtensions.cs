using Agronomia.Domain.Aggregates.Users.ValueObjects;

namespace Agronomia.Api.Extensions.Auth;

public static class AuthorizationExtensions
{
    public static WebApplicationBuilder AddAuthorizationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("User", policy => policy.RequireAuthenticatedUser())
            .AddPolicy("Administrator", policy => policy.RequireRole(UserRole.Administrator.ToString()));

        return builder;
    }
}
