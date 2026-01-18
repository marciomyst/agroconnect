using Agronomia.Api.Features.Auth.Login;
using Swashbuckle.AspNetCore.Filters;

namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Provides Swagger examples for <see cref="LoginRequest"/>.
/// </summary>
public sealed class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    /// <summary>
    /// Example payload for user login.
    /// </summary>
    public LoginRequest GetExamples() => new("user@example.com", "P@ssw0rd!", "device-web-123456");
}
