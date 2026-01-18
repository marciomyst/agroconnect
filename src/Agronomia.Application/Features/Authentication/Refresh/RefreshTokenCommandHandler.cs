using Agronomia.Crosscutting.Security;
using Agronomia.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Agronomia.Application.Features.Authentication.Refresh;

/// <summary>
/// Handles refresh token exchange, issuing a new access/refresh pair.
/// </summary>
/// <param name="cache">Cache used to resolve and store refresh tokens.</param>
/// <param name="authenticationReadRepository">Repository to load the user.</param>
/// <param name="jwtOptions">JWT settings.</param>
public sealed class RefreshTokenCommandHandler(ICacheService cache, IAuthenticationReadRepository authenticationReadRepository, IOptions<JwtTokenSettings> jwtOptions)
{
    private readonly JwtTokenSettings _jwtSettings = jwtOptions.Value;

    /// <inheritdoc />
    public async Task<RefreshTokenResult?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = GetRefreshCacheKey(request.RefreshToken);
        Guid? userId = await cache.GetAsync<Guid>(cacheKey);
        if (userId is null || userId.Value == Guid.Empty)
        {
            return null;
        }

        var user = await authenticationReadRepository.GetUserByIdAsync(userId.Value, cancellationToken);
        if (user is null)
        {
            await cache.RemoveAsync(cacheKey);
            return null;
        }

        // rotate refresh token
        await cache.RemoveAsync(cacheKey);

        string newRefresh = Guid.NewGuid().ToString("N");
        await cache.SetAsync(GetRefreshCacheKey(newRefresh), user.Id, TimeSpan.FromDays(30));

        string accessToken = JwtTokenGenerator.Generate(user, jwtOptions.Value);

        return new RefreshTokenResult(accessToken, newRefresh);
    }

    private static string GetRefreshCacheKey(string refreshToken) => $"refresh:{refreshToken}";
}
