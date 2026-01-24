using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Application.Features.Identity.RegisterUser;
using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;
using Xunit;

namespace Agronomia.Application.Tests.Identity;

public sealed class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DuplicateEmail_ThrowsConflict()
    {
        var repository = new FakeUserRepository { ExistsResult = true };
        var handler = new RegisterUserCommandHandler(repository, new FakePasswordHasher(), new FakeUnitOfWork());

        var command = new RegisterUserCommand("User", "user@example.com", "password123");

        await Assert.ThrowsAsync<UserEmailAlreadyExistsException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PasswordIsHashed()
    {
        var repository = new FakeUserRepository();
        var hasher = new FakePasswordHasher();
        var handler = new RegisterUserCommandHandler(repository, hasher, new FakeUnitOfWork());

        var command = new RegisterUserCommand("User", "user@example.com", "password123");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotNull(repository.AddedUser);
        Assert.NotEqual(command.Password, repository.AddedUser!.PasswordHash);
        Assert.Equal(hasher.Hash(command.Password), repository.AddedUser!.PasswordHash);
        Assert.Equal("user@example.com", result.Email);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public bool ExistsResult { get; set; }

        public User? AddedUser { get; private set; }

        public Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task<User?> GetByEmailAsync(Email email, CancellationToken ct)
        {
            return Task.FromResult<User?>(null);
        }

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return Task.FromResult<User?>(null);
        }

        public Task<bool> ExistsAsync(Guid userId, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task AddAsync(User user, CancellationToken ct)
        {
            AddedUser = user;
            return Task.CompletedTask;
        }
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => $"hashed::{password}";

        public bool Verify(string password, string passwordHash)
        {
            return passwordHash == $"hashed::{password}";
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task BeginTransactionAsync(CancellationToken ct) => Task.CompletedTask;

        public Task CommitAsync(CancellationToken ct) => Task.CompletedTask;

        public Task RollbackAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
