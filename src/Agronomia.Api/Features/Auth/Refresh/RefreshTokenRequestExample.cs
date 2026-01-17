using Agronomia.Api.Features.Auth.Refresh;
using Swashbuckle.AspNetCore.Filters;

namespace Agronomia.Api.Features.Auth.Refresh;

/// <summary>
/// Provides Swagger examples for <see cref="RefreshTokenRequest"/>.
/// </summary>
public sealed class RefreshTokenRequestExample : IExamplesProvider<RefreshTokenRequest>
{
    /// <summary>
    /// Example payload for refreshing a JWT.
    /// </summary>
    public RefreshTokenRequest GetExamples() => new("b27f8a9c0c9d4a2d9c1f4fb9c8e2c1f3");
}
