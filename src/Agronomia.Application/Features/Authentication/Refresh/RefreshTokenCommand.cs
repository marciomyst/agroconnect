namespace Agronomia.Application.Features.Authentication.Refresh;

/// <summary>
/// Command to exchange a refresh token for a new access token.
/// </summary>
/// <param name="RefreshToken">Refresh token issued on login.</param>
/// <param name="DeviceId">Client-provided identifier for the device/session.</param>
public sealed record RefreshTokenCommand(string RefreshToken, string DeviceId);
