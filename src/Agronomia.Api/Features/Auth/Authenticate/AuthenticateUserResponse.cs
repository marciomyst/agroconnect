namespace Agronomia.Api.Features.Auth.Authenticate;

public sealed record AuthenticateUserResponse(
    Guid UserId,
    string Email,
    string AccessToken
);
