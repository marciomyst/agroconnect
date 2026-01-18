using Agronomia.Domain.Aggregates.Sellers;
using Agronomia.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework Core repository for <see cref="Seller"/> aggregates.
/// </summary>
/// <param name="context">Agronomia DbContext instance.</param>
public sealed class SellerRepository(AgronomiaDbContext context) : ISellerRepository
{
    /// <inheritdoc />
    public IUnitOfWork UnitOfWork => context;

    /// <inheritdoc />
    public void Add(Seller seller)
    {
        context.Sellers.Add(seller);
    }

    /// <inheritdoc />
    public void Update(Seller seller)
    {
        context.Sellers.Update(seller);
    }

    /// <inheritdoc />
    public Task<Seller?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return context.Sellers
            .Include(s => s.Managers)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
