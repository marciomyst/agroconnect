using Agronomia.Domain.Interfaces;

namespace Agronomia.Application.Features.Authentication.Logout;

/// <summary>
/// Handles refresh token revocation.
/// </summary>
/// <param name="cache">Cache service storing issued refresh tokens.</param>
public sealed class LogoutCommandHandler(ICacheService cache)
{
    /// <inheritdoc />
    public async Task<LogoutResult?> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string cacheKey = GetRefreshCacheKey(command.RefreshToken);
        await cache.RemoveAsync(cacheKey);
        return new LogoutResult(true);
    }

    private static string GetRefreshCacheKey(string refreshToken) => $"refresh:{refreshToken}";
}
