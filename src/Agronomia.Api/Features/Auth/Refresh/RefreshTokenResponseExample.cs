using Agronomia.Api.Features.Auth.Login;
using Swashbuckle.AspNetCore.Filters;

namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Provides Swagger examples for refresh token responses.
/// </summary>
public sealed class RefreshTokenResponseExample : IExamplesProvider<LoginResponse>
{
    /// <summary>
    /// Example response containing new access and refresh tokens.
    /// </summary>
    public LoginResponse GetExamples() =>
        new("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", "f4fb9c8e2c1f3b27f8a9c0c9d4a2d9c1");
}
