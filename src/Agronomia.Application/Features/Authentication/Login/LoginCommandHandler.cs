using Agronomia.Crosscutting.Security;
using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Agronomia.Application.Features.Authentication.Login;

/// <summary>
/// Handles user authentication by validating credentials and issuing tokens.
/// </summary>
/// <param name="authenticationReadRepository">Repository used to retrieve user aggregates.</param>
/// <param name="jwtOptions">JWT configuration bound from application settings.</param>
public sealed class LoginCommandHandler(IAuthenticationReadRepository authenticationReadRepository, IOptions<JwtTokenSettings> jwtOptions, ICacheService cache)
{
    private static string GetRefreshCacheKey(string refreshToken) => $"refresh:{refreshToken}";

    public async Task<LoginResult?> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await authenticationReadRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (user is null)
        {
            return null;
        }

        if (!PasswordHasher.Verify(user.Password, command.Password))
        {
            return null;
        }

        string token = JwtTokenGenerator.Generate(user, jwtOptions.Value);
        string refreshToken = Guid.NewGuid().ToString("N");

        string cacheKey = GetRefreshCacheKey(refreshToken);
        await cache.SetAsync(cacheKey, user.Id, TimeSpan.FromDays(30));

        return new LoginResult(token, refreshToken);
    }
}
