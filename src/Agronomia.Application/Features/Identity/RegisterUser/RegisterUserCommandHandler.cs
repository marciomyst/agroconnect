using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;

namespace Agronomia.Application.Features.Identity.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<RegisterUserResult> HandleAsync(RegisterUserCommand command, CancellationToken ct)
    {
        var email = Email.Create(command.Email);

        if (await _userRepository.ExistsByEmailAsync(email, ct))
        {
            throw new UserEmailAlreadyExistsException(email.Value);
        }

        var passwordHash = _passwordHasher.Hash(command.Password);
        var user = User.Register(command.Name, email, passwordHash);

        await _userRepository.AddAsync(user, ct);

        return new RegisterUserResult(user.Id, user.Email.Value);
    }
}
