namespace Agronomia.Api.Features.Users.RegisterUser;

public sealed record RegisterUserResponse(
    Guid UserId,
    string Email
);
