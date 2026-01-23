using Agronomia.Domain.Catalog.Products;
using Agronomia.Domain.Catalog.SellerProducts;
using Agronomia.Domain.Identity;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Orders.PurchaseIntents;
using Agronomia.Domain.Organizations;
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
    /// DbSet for <see cref="Seller"/> aggregate roots.
    /// </summary>
    public DbSet<Seller> Sellers => Set<Seller>();

    /// <summary>
    /// DbSet for <see cref="SellerMembership"/> aggregate roots.
    /// </summary>
    public DbSet<SellerMembership> SellerMemberships => Set<SellerMembership>();

    /// <summary>
    /// DbSet for <see cref="Farm"/> aggregate roots.
    /// </summary>
    public DbSet<Farm> Farms => Set<Farm>();

    /// <summary>
    /// DbSet for <see cref="FarmMembership"/> aggregate roots.
    /// </summary>
    public DbSet<FarmMembership> FarmMemberships => Set<FarmMembership>();

    /// <summary>
    /// DbSet for <see cref="Product"/> aggregate roots.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// DbSet for <see cref="SellerProduct"/> aggregate roots.
    /// </summary>
    public DbSet<SellerProduct> SellerProducts => Set<SellerProduct>();

    /// <summary>
    /// DbSet for <see cref="PurchaseIntent"/> aggregate roots.
    /// </summary>
    public DbSet<PurchaseIntent> PurchaseIntents => Set<PurchaseIntent>();

    /// <summary>
    /// Applies all IEntityTypeConfiguration classes in this assembly.
    /// <para>Ensures the model picks up fluent configurations for aggregates.</para>
    /// </summary>
    /// <param name="modelBuilder">Builder used to configure the EF Core model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Ignore<DomainEvent>();

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
