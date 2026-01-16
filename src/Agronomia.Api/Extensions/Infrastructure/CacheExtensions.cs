namespace Agronomia.Api.Extensions.Infrastructure;

public static class CacheExtensions
{
    public static WebApplicationBuilder AddCacheServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        //builder.Services.AddScoped<ICacheService, MemoryCacheService>();

        return builder;
    }
}
