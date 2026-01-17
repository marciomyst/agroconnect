using Agronomia.Api.Features.Auth.Refresh;
using Swashbuckle.AspNetCore.Filters;

namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Provides Swagger examples for <see cref="RefreshTokenResponse"/>.
/// </summary>
public sealed class LoginResponseExample : IExamplesProvider<RefreshTokenResponse>
{
    /// <summary>
    /// Example response returned after successful authentication.
    /// </summary>
    public RefreshTokenResponse GetExamples() =>
        new("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", "b27f8a9c0c9d4a2d9c1f4fb9c8e2c1f3");
}
