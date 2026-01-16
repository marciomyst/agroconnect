using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Agronomia.Crosscutting.Security;

/// <summary>
/// Issues JWT tokens for authenticated users using the same parameters as the API middleware.
/// </summary>
public abstract class JwtTokenGenerator(JwtTokenSettings settings)
{
    public abstract IEnumerable<Claim> GenerateClaims();

    public string GenerateToken()
    {
        var claims = GenerateClaims();
        ArgumentNullException.ThrowIfNull(claims);

        DateTimeOffset issuedAt = DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = issuedAt.AddMinutes(settings.ExpiresInMinutes <= 0 ? 60 : settings.ExpiresInMinutes);

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
