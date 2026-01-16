using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Agronomia.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for creating <see cref="AgronomiaDbContext"/> instances when running EF Core tools.
/// </summary>
internal sealed class AgronomiaDbContextFactory : IDesignTimeDbContextFactory<AgronomiaDbContext>
{
    public AgronomiaDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string connectionString = configuration.GetConnectionString("WriteDatabase")
            ?? "Host=localhost;Port=5432;Database=agronomia;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<AgronomiaDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AgronomiaDbContext(optionsBuilder.Options);
    }
}
