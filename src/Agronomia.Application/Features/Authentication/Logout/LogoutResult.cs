namespace Agronomia.Application.Features.Authentication.Logout;

/// <summary>
/// Result of a logout operation.
/// </summary>
/// <param name="Revoked">Indicates whether the refresh token was revoked.</param>
public sealed record LogoutResult(bool Revoked);
