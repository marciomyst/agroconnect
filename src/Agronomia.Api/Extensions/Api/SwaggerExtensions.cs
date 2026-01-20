using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Agronomia.Api.Extensions.Api;

public static class SwaggerExtensions
{
    public static WebApplicationBuilder AddSwaggerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Agronomia API",
                Version = "v1",
                Description = "Agronomia backend API (minimal APIs + CQRS/MediatR) with JWT authentication and multi-tenant company endpoints."
            });

            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlPath);

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Bearer token. Ex: 'Bearer {token}'"
            };

            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", doc, null),
                    new List<string>()
                }
            });
        });

        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        return builder;
    }

    public static WebApplication UseSwaggerDocs(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
