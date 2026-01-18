using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Agronomia.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core DbContext for the write model.
/// </summary>
public class AgronomiaDbContext : DbContext, IUnitOfWork
{
    public AgronomiaDbContext(DbContextOptions<AgronomiaDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet for <see cref="User"/> aggregate roots.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Applies all IEntityTypeConfiguration classes in this assembly.
    /// <para>Ensures the model picks up fluent configurations for aggregates.</para>
    /// </summary>
    /// <param name="modelBuilder">Builder used to configure the EF Core model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <param name="cancellationToken">Token to observe while saving.</param>
    /// <returns><c>true</c> when the save completes successfully.</returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}
