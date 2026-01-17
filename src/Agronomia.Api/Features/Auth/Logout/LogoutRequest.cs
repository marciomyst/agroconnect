using Agronomia.Application.Features.Authentication.Logout;

namespace Agronomia.Api.Features.Auth.Logout;

/// <summary>
/// Payload to revoke a refresh token.
/// </summary>
/// <param name="RefreshToken">Refresh token issued during authentication.</param>
public sealed record LogoutRequest(string RefreshToken)
{
    /// <summary>
    /// Converts the request into a logout command.
    /// </summary>
    public LogoutCommand ToCommand() => new(RefreshToken);
}
