namespace Agronomia.Application.Features.Authentication.Refresh;

/// <summary>
/// Command to exchange a refresh token for a new access token.
/// </summary>
/// <param name="RefreshToken">Refresh token issued on login.</param>
public sealed record RefreshTokenCommand(string RefreshToken);
