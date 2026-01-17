namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Represents the response data from a successful user login, containing the JWT authentication token.
/// </summary>
/// <param name="Token">The JWT bearer token issued after successful authentication.</param>
/// <param name="RefreshToken">Refresh token to obtain new access tokens.</param>
public record LoginResponse(
    string Token,
    string RefreshToken);
