namespace Agronomia.Application.Features.Authentication.Login;

/// <summary>
/// Command to authenticate a user and issue access/refresh tokens.
/// </summary>
/// <param name="Email">User email used as the login identifier.</param>
/// <param name="Password">Plain text password provided by the user.</param>
/// <param name="DeviceId">Client-provided identifier for the device/session.</param>
public sealed record LoginCommand(string Email, string Password, string DeviceId);
