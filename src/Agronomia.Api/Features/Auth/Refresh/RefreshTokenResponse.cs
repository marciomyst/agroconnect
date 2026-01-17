namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Represents the response data from a refresh authentication, containing the JWT authentication token.
/// </summary>
/// <param name="Token">The JWT bearer token issued after successful authentication.</param>
/// <param name="RefreshToken">Refresh token to obtain new access tokens.</param>
public record RefreshTokenResponse(
    string Token,
    string RefreshToken);
