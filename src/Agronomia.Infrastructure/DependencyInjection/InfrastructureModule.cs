using System.Data;
using Agronomia.Infrastructure.Dapper;
using Agronomia.Infrastructure.Persistence;
using Agronomia.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApplicationUnitOfWork = Agronomia.Application.Abstractions.Persistence.IUnitOfWork;
using DomainUnitOfWork = Agronomia.Domain.Common.IUnitOfWork;

namespace Agronomia.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WriteDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:WriteDatabase is not configured.");
        }

        services.AddDbContext<AgronomiaDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<ApplicationUnitOfWork, EfUnitOfWork>();
        services.AddScoped<DomainUnitOfWork>(sp => sp.GetRequiredService<AgronomiaDbContext>());

        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

        return services;
    }
}
