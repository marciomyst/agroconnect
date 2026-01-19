using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Abstractions.Security;

namespace Agronomia.Application.Features.Identity.ChangeUserPassword;

public sealed class ChangeUserPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeUserPasswordCommand, ChangeUserPasswordResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ChangeUserPasswordResult> HandleAsync(ChangeUserPasswordCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null || !_passwordHasher.Verify(command.CurrentPassword, user.PasswordHash))
        {
            throw new InvalidCurrentPasswordException();
        }

        var newPasswordHash = _passwordHasher.Hash(command.NewPassword);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            user.ChangePassword(newPasswordHash);
            await _unitOfWork.CommitAsync(ct);
            return new ChangeUserPasswordResult(user.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
