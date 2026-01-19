using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Agronomia.Application.Abstractions.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Agronomia.Infrastructure.Auth;

public sealed class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    private const int DefaultExpiresInMinutes = 60;

    private readonly string _issuer = configuration["Jwt:Issuer"]
        ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");

    private readonly string _audience = configuration["Jwt:Audience"]
        ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

    private readonly string _secret = configuration["Jwt:Secret"]
        ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

    private readonly int _expiresInMinutes = ResolveExpiresInMinutes(configuration);

    public string GenerateToken(Guid userId, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        var now = DateTimeOffset.UtcNow;
        var expires = now.AddMinutes(_expiresInMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static int ResolveExpiresInMinutes(IConfiguration configuration)
    {
        var value = configuration["Jwt:ExpiresInMinutes"];
        return int.TryParse(value, out var minutes) && minutes > 0 ? minutes : DefaultExpiresInMinutes;
    }
}
