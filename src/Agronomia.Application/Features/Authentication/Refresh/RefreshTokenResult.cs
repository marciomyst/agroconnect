namespace Agronomia.Application.Features.Authentication.Refresh;

/// <summary>
/// Result of a successful authentication refresh attempt.
/// </summary>
/// <param name="Token">JWT bearer token for API access.</param>
/// <param name="RefreshToken">Opaque refresh token to renew access tokens.</param>
public sealed record RefreshTokenResult(string Token, string RefreshToken);
