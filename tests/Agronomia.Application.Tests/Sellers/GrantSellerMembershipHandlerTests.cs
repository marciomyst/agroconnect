using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Features.Sellers.GrantSellerMembership;
using Agronomia.Domain.Memberships;
using Xunit;

namespace Agronomia.Application.Tests.Sellers;

public sealed class GrantSellerMembershipHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidOwner_GrantsMembership()
    {
        var sellerId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var sellerRepository = new FakeSellerRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeSellerMembershipRepository
        {
            ExecutorMemberships = [SellerMembership.GrantOwner(sellerId, executorUserId)]
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantSellerMembershipHandler(
            sellerRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantSellerMembershipCommand(executorUserId, sellerId, targetUserId, SellerRole.Manager);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.SellerMembershipId);
        Assert.NotNull(membershipRepository.AddedMembership);
        Assert.Equal(targetUserId, membershipRepository.AddedMembership!.UserId);
        Assert.Equal(SellerRole.Manager, membershipRepository.AddedMembership!.Role);
        Assert.Equal(1, unitOfWork.BeginCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_ExecutorNotOwner_ThrowsForbidden()
    {
        var sellerId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var sellerRepository = new FakeSellerRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeSellerMembershipRepository
        {
            ExecutorMemberships = [SellerMembership.Grant(sellerId, executorUserId, SellerRole.Manager)]
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantSellerMembershipHandler(
            sellerRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantSellerMembershipCommand(executorUserId, sellerId, targetUserId, SellerRole.Viewer);

        await Assert.ThrowsAsync<SellerMembershipForbiddenException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_MembershipAlreadyExists_ThrowsConflict()
    {
        var sellerId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var sellerRepository = new FakeSellerRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeSellerMembershipRepository
        {
            ExecutorMemberships = [SellerMembership.GrantOwner(sellerId, executorUserId)],
            ExistsResult = true
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantSellerMembershipHandler(
            sellerRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantSellerMembershipCommand(executorUserId, sellerId, targetUserId, SellerRole.Manager);

        await Assert.ThrowsAsync<SellerMembershipAlreadyExistsException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_SellerNotFound_Throws()
    {
        var sellerRepository = new FakeSellerRepository { ExistsResult = false };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeSellerMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantSellerMembershipHandler(
            sellerRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantSellerMembershipCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), SellerRole.Viewer);

        await Assert.ThrowsAsync<SellerNotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_TargetUserNotFound_Throws()
    {
        var sellerId = Guid.NewGuid();

        var sellerRepository = new FakeSellerRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = false };
        var membershipRepository = new FakeSellerMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantSellerMembershipHandler(
            sellerRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantSellerMembershipCommand(Guid.NewGuid(), sellerId, Guid.NewGuid(), SellerRole.Viewer);

        await Assert.ThrowsAsync<UserNotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    private sealed class FakeSellerRepository : ISellerRepository
    {
        public bool ExistsResult { get; set; }

        public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ExistsAsync(Guid sellerId, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task AddAsync(Agronomia.Domain.Organizations.Seller seller, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public bool ExistsResult { get; set; }

        public Task<bool> ExistsByEmailAsync(Agronomia.Domain.Identity.ValueObjects.Email email, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task<Agronomia.Domain.Identity.User?> GetByEmailAsync(
            Agronomia.Domain.Identity.ValueObjects.Email email,
            CancellationToken ct)
        {
            return Task.FromResult<Agronomia.Domain.Identity.User?>(null);
        }

        public Task<Agronomia.Domain.Identity.User?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return Task.FromResult<Agronomia.Domain.Identity.User?>(null);
        }

        public Task<bool> ExistsAsync(Guid userId, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task AddAsync(Agronomia.Domain.Identity.User user, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeSellerMembershipRepository : ISellerMembershipRepository
    {
        public IReadOnlyList<SellerMembership> ExecutorMemberships { get; set; } = [];

        public bool ExistsResult { get; set; }

        public SellerMembership? AddedMembership { get; private set; }

        public Task<IReadOnlyList<SellerMembership>> GetBySellerAndUserAsync(
            Guid sellerId,
            Guid userId,
            CancellationToken ct)
        {
            return Task.FromResult(ExecutorMemberships);
        }

        public Task<bool> ExistsAsync(Guid sellerId, Guid userId, SellerRole role, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task AddAsync(SellerMembership membership, CancellationToken ct)
        {
            AddedMembership = membership;
            return Task.CompletedTask;
        }
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
