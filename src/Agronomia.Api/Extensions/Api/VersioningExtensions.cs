using Asp.Versioning;

namespace Agronomia.Api.Extensions.Api;

public static class VersioningExtensions
{
    public static WebApplicationBuilder AddVersioningServices(this WebApplicationBuilder builder)
    {
        var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;

            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);

            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version")
            );
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return builder;
    }
}