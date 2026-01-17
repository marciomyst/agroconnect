using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Agronomia.Domain.Aggregates.Users;
using Microsoft.IdentityModel.Tokens;
using Agronomia.Crosscutting.Security;

namespace Agronomia.Application.Features.Authentication;

/// <summary>
/// Issues JWT tokens for authenticated users using the same parameters as the API middleware.
/// </summary>
public static class JwtTokenGenerator
{
    public static string Generate(AuthenticationUserDto user, JwtTokenSettings settings, DateTimeOffset? now = null)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(settings);
        if (string.IsNullOrWhiteSpace(settings.Secret))
        {
            throw new ArgumentException("JWT secret must be provided.", nameof(settings));
        }

        DateTimeOffset issuedAt = now ?? DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = issuedAt.AddMinutes(settings.ExpiresInMinutes <= 0 ? 60 : settings.ExpiresInMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, user.Name),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: issuedAt.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
