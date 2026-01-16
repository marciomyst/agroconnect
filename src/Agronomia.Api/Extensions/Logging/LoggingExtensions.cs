using Microsoft.AspNetCore.HttpLogging;

namespace Agronomia.Api.Extensions.Logging;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLoggingServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders
                | HttpLoggingFields.ResponsePropertiesAndHeaders;
        });

        return builder;
    }
}
