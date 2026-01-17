using Agronomia.Application.Features.Authentication.Refresh;

namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Maps refresh command results to API responses.
/// </summary>
public static class RefreshTokenMapper
{
    /// <summary>
    /// Converts a refresh token result into a new refresh token response.
    /// </summary>
    /// <param name="result">Login result with tokens.</param>
    public static RefreshTokenResponse FromResult(this RefreshTokenResult result) =>
        new(result.Token, result.RefreshToken);
}
