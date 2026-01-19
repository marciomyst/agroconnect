namespace Agronomia.Api.Features.Auth.Authenticate;

public sealed record AuthenticateUserRequest(
    string Email,
    string Password
);
