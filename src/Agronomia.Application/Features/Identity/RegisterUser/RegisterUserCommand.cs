using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Identity.RegisterUser;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string Password
) : ICommand<RegisterUserResult>;
