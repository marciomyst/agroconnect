using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Agronomia.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core DbContext for the Agronomia domain.
/// <para>
/// Implements <see cref="IUnitOfWork"/> so changes to aggregates are persisted in a single transaction,
/// and ensures domain events are dispatched before committing.
/// </para>
/// <para>
/// Exposes DbSets only for aggregate roots and
/// applies configurations from this assembly, and publishes domain events through MediatR prior to saving.
/// </para>
/// </summary>
/// <param name="options">DbContext options configured via dependency injection.</param>
/// <param name="mediator">Mediator used to publish domain events raised by aggregates.</param>
public class AgronomiaDbContext(DbContextOptions<AgronomiaDbContext> options)
    : DbContext(options), IUnitOfWork
{
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
    /// Persists changes and dispatches pending domain events prior to committing.
    /// </summary>
    /// <param name="cancellationToken">Token to observe while saving.</param>
    /// <returns><c>true</c> when the save completes successfully.</returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}
