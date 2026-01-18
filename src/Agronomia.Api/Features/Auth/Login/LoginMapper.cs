using Agronomia.Api.Features.Auth.Refresh;
using Agronomia.Application.Features.Authentication.Login;

namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Provides mapping functionality between API requests/responses and application commands/DTOs for the login feature.
/// </summary>
public static class LoginMapper
{
    /// <summary>
    /// Converts a <see cref="LoginRequest"/> to an application command for processing.
    /// </summary>
    /// <param name="request">The login request data.</param>
    /// <returns>The application command.</returns>
    /// <remarks>
    /// Builds the application login command.
    /// </remarks>
    public static LoginCommand ToCommand(this LoginRequest request)
    {
        return new LoginCommand(request.Email, request.Password, request.DeviceId);
    }

    /// <summary>
    /// Converts a login data transfer object to a <see cref="RefreshTokenResponse"/> for the API response.
    /// </summary>
    /// <param name="result">The login result.</param>
    /// <returns>The login response.</returns>
    /// <remarks>
    /// The mapper assumes the DTO contains a populated JWT token issued by the authentication provider.
    /// </remarks>
    public static RefreshTokenResponse FromResult(this LoginResult result)
    {
        return new RefreshTokenResponse(result.Token, result.RefreshToken);
    }
}
