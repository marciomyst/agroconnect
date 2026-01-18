using Agronomia.Domain.Common;

namespace Agronomia.Domain.Aggregates.Users;

/// <summary>
/// Repository contract for managing <see cref="User"/> aggregates.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Unit of work tied to this repository.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adds a new user aggregate to the store.
    /// </summary>
    /// <param name="user">User instance to add.</param>
    void Add(User user);

    /// <summary>
    /// Updates an existing user aggregate.
    /// </summary>
    /// <param name="user">User instance to update.</param>
    void Update(User user);

    /// <summary>
    /// Retrieves a user by its identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise <c>null</c>.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
