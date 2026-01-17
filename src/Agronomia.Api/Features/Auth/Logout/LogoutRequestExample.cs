using Swashbuckle.AspNetCore.Filters;

namespace Agronomia.Api.Features.Auth.Logout;

/// <summary>
/// Swagger example for logout requests.
/// </summary>
public sealed class LogoutRequestExample : IExamplesProvider<LogoutRequest>
{
    /// <inheritdoc />
    public LogoutRequest GetExamples() =>
        new("b27f8a9c0c9d4a2d9c1f4fb9c8e2c1f3");
}
