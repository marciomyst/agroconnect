namespace Agronomia.Api.Extensions.Auth;

public static class AuthorizationExtensions
{
    public static WebApplicationBuilder AddAuthorizationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("User", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin", "Administrator"));
        });

        return builder;
    }
}
