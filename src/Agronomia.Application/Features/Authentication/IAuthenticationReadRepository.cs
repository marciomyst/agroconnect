namespace Agronomia.Application.Features.Authentication;

/// <summary>
/// Read model repository for user projections using optimized queries (e.g., Dapper).
/// </summary>
public interface IAuthenticationReadRepository
{
    /// <summary>
    /// Retrieves a user by email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise <c>null</c>.</returns>
    Task<AuthenticationUserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user projection by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User projection when found; otherwise null.</returns>
    Task<AuthenticationUserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
}
