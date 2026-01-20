using Agronomia.Infrastructure.DependencyInjection;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class DbContextExtensions
{
    public static WebApplicationBuilder AddDbContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration);
        return builder;
    }
}
