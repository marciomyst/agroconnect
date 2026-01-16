namespace Agronomia.Crosscutting.Security;

/// <summary>
/// Settings required to issue JWT tokens consistently with the API authentication configuration.
/// </summary>
public sealed class JwtTokenSettings
{
    /// <summary>
    /// Issuer value configured in Jwt:Issuer.
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Audience value configured in Jwt:Audience.
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    /// Symmetric secret configured in Jwt:Secret.
    /// </summary>
    public required string Secret { get; init; }

    /// <summary>
    /// Token lifetime in minutes. Defaults to 60.
    /// </summary>
    public int ExpiresInMinutes { get; init; } = 60;
}