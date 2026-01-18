namespace Agronomia.Api.Features.Auth.Login;

/// <summary>
/// Represents the request data for user authentication, containing the user's email and password.
/// </summary>
/// <param name="Email">The email address that identifies the user account.</param>
/// <param name="Password">The user's password in plain text; it is validated and never returned.</param>
/// <param name="DeviceId">Client-provided identifier to bind refresh tokens to this device/session.</param>
public record LoginRequest(
    string Email,
    string Password,
    string DeviceId);
