using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Application.Features.Identity.ChangeUserPassword;
using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;
using Xunit;

namespace Agronomia.Application.Tests.Identity;

public sealed class ChangeUserPasswordCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_InvalidCurrentPassword_Throws()
    {
        var user = CreateUser("old-hash");
        var repository = new FakeUserRepository(user);
        var hasher = new FakePasswordHasher(isValid: false);
        var unitOfWork = new FakeUnitOfWork();
        var handler = new ChangeUserPasswordCommandHandler(repository, hasher, unitOfWork);

        var command = new ChangeUserPasswordCommand(user.Id, "wrong", "new-password");

        await Assert.ThrowsAsync<InvalidCurrentPasswordException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_UpdatesPasswordAndCommits()
    {
        var user = CreateUser("old-hash");
        var repository = new FakeUserRepository(user);
        var hasher = new FakePasswordHasher(isValid: true);
        var unitOfWork = new FakeUnitOfWork();
        var handler = new ChangeUserPasswordCommandHandler(repository, hasher, unitOfWork);

        var command = new ChangeUserPasswordCommand(user.Id, "old", "new-password");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(user.Id, result.UserId);
        Assert.Equal("hashed::new-password", user.PasswordHash);
        Assert.Equal(1, unitOfWork.BeginCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    private static User CreateUser(string passwordHash)
    {
        var email = Email.Create("user@example.com");
        return User.Register("User", email, passwordHash);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly User _user;

        public FakeUserRepository(User user)
        {
            _user = user;
        }

        public Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task<User?> GetByEmailAsync(Email email, CancellationToken ct)
        {
            return Task.FromResult<User?>(null);
        }

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return Task.FromResult(userId == _user.Id ? _user : null);
        }

        public Task AddAsync(User user, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        private readonly bool _isValid;

        public FakePasswordHasher(bool isValid)
        {
            _isValid = isValid;
        }

        public string Hash(string password) => $"hashed::{password}";

        public bool Verify(string password, string passwordHash) => _isValid;
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int BeginCalls { get; private set; }

        public int CommitCalls { get; private set; }

        public int RollbackCalls { get; private set; }

        public Task BeginTransactionAsync(CancellationToken ct)
        {
            BeginCalls++;
            return Task.CompletedTask;
        }

        public Task CommitAsync(CancellationToken ct)
        {
            CommitCalls++;
            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken ct)
        {
            RollbackCalls++;
            return Task.CompletedTask;
        }
    }
}
