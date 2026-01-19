namespace Agronomia.Application.Features.Identity.AuthenticateUser;

public sealed record AuthenticateUserResult(
    Guid UserId,
    string Email,
    string AccessToken
);
