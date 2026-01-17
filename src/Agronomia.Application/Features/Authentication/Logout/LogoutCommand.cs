namespace Agronomia.Application.Features.Authentication.Logout;

/// <summary>
/// Command to revoke a refresh token.
/// </summary>
/// <param name="RefreshToken">Refresh token to invalidate.</param>
public sealed record LogoutCommand(string RefreshToken);
