using Agronomia.Application.Abstractions.Auth;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Messaging;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Domain.Identity.Events;
using Agronomia.Domain.Identity.ValueObjects;

namespace Agronomia.Application.Features.Identity.AuthenticateUser;

public sealed class AuthenticateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IEventDispatcher eventDispatcher)
    : ICommandHandler<AuthenticateUserCommand, AuthenticateUserResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    public async Task<AuthenticateUserResult> HandleAsync(AuthenticateUserCommand command, CancellationToken ct)
    {
        var email = Email.Create(command.Email);
        var user = await _userRepository.GetByEmailAsync(email, ct);

        if (user is null || !_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email.Value);

        var authenticatedAtUtc = DateTime.UtcNow;
        var domainEvent = new UserAuthenticated(
            Guid.NewGuid(),
            authenticatedAtUtc,
            user.Id,
            authenticatedAtUtc);
        await _eventDispatcher.DispatchAsync([domainEvent], ct);

        return new AuthenticateUserResult(user.Id, user.Email.Value, token);
    }
}
