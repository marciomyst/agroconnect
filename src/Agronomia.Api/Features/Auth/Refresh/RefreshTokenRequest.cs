using Agronomia.Application.Features.Authentication.Refresh;

namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Request payload for refresh token exchange.
/// </summary>
/// <param name="RefreshToken">Refresh token issued during login.</param>
/// <param name="DeviceId">Client-provided identifier for the device/session.</param>
public sealed record RefreshTokenRequest(string RefreshToken, string DeviceId)
{
    /// <summary>
    /// Converts the request into an application command.
    /// </summary>
    /// <returns>Command to refresh the access token.</returns>
    public RefreshTokenCommand ToCommand() => new(RefreshToken, DeviceId);
}
