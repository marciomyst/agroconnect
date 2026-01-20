namespace Agronomia.Application.Features.Identity.RegisterUser;

public sealed record RegisterUserResult(
    Guid UserId,
    string Email
);
