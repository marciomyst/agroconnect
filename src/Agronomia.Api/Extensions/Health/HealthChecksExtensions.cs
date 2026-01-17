namespace Agronomia.Api.Extensions.Health;

public static class HealthChecksExtensions
{
    public static WebApplicationBuilder AddHealthChecksServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

        return builder;
    }

    public static WebApplication MapHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        return app;
    }
}
