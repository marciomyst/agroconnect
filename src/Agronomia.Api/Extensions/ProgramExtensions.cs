using Agronomia.Api.Extensions.Api;
using Agronomia.Api.Extensions.Application;
using Agronomia.Api.Extensions.Auth;
using Agronomia.Api.Extensions.Health;
using Agronomia.Api.Extensions.Infrastructure;
using Agronomia.Api.Extensions.Logging;
using Agronomia.Api.Extensions.Mapping;
using Agronomia.Api.Extensions.Messaging;

namespace Agronomia.Api.Extensions;

public static class ProgramExtensions
{
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        builder.AddApiServices()
            .AddSwaggerServices()
            .AddVersioningServices()
            .AddMediatorServices()
            .AddValidators()
            .AddApplicationServices()
            .AddAuthenticationServices()
            .AddAuthorizationServices()
            .AddHealthChecksServices()
            .AddDbContextServices()
            .AddRepositoryServices()
            .AddCacheServices()
            .AddExternalClientServices()
            .AddLoggingServices()
            .AddAutoMapperServices()
            .AddMessagingServices();

        return builder;
    }

    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        app.UseApiPipeline()
            .UseSwaggerDocs();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthEndpoints();
        app.MapApiEndpoints();
        app.MapFallbackToFile("index.html");

        return app;
    }
}
