using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Identity.AuthenticateUser;

public sealed record AuthenticateUserCommand(
    string Email,
    string Password
) : ICommand<AuthenticateUserResult>;
