namespace Agronomia.Application.Features.Users;

/// <summary>
/// Read model repository for user projections using optimized queries (e.g., Dapper).
/// </summary>
public interface IUserReadRepository
{
    /// <summary>
    /// Retrieves a user projection by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User projection when found; otherwise null.</returns>
    Task<UserDto?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
}
