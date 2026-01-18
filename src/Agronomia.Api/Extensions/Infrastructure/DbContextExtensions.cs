using Agronomia.Domain.Common;
using Agronomia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class DbContextExtensions
{
    public static WebApplicationBuilder AddDbContextServices(this WebApplicationBuilder builder)
    {
        var writeConnectionString = builder.Configuration.GetConnectionString("WriteDatabase");
        if (string.IsNullOrWhiteSpace(writeConnectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:WriteDatabase is not configured.");
        }

        builder.Services.AddDbContext<AgronomiaDbContext>(options => options.UseNpgsql(writeConnectionString));

        var readConnectionString = builder.Configuration.GetConnectionString("ReadDatabase");
        if (string.IsNullOrWhiteSpace(readConnectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:ReadDatabase is not configured.");
        }

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(readConnectionString));
        builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AgronomiaDbContext>());

        return builder;
    }
}
