namespace Agronomia.Application.Features.Authentication.Login;

/// <summary>
/// Result of a successful authentication attempt.
/// </summary>
/// <param name="Token">JWT bearer token for API access.</param>
/// <param name="RefreshToken">Opaque refresh token to renew access tokens.</param>
public sealed record LoginResult(string Token, string RefreshToken);
