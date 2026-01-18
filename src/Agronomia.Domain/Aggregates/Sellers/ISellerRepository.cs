using Agronomia.Domain.SeedWork;

namespace Agronomia.Domain.Aggregates.Sellers;

/// <summary>
/// Repository contract for <see cref="Seller"/> aggregates.
/// </summary>
public interface ISellerRepository
{
    /// <summary>
    /// Unit of work bound to this repository.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adds a new seller aggregate.
    /// </summary>
    void Add(Seller seller);

    /// <summary>
    /// Updates an existing seller aggregate.
    /// </summary>
    void Update(Seller seller);

    /// <summary>
    /// Retrieves a seller by identifier.
    /// </summary>
    Task<Seller?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
