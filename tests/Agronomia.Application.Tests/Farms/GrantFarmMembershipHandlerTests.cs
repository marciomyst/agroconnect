using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Features.Farms.GrantFarmMembership;
using Agronomia.Domain.Memberships;
using Xunit;

namespace Agronomia.Application.Tests.Farms;

public sealed class GrantFarmMembershipHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidOwner_GrantsMembership()
    {
        var farmId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var farmRepository = new FakeFarmRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeFarmMembershipRepository
        {
            ExecutorMemberships = [FarmMembership.GrantOwner(farmId, executorUserId)]
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantFarmMembershipHandler(
            farmRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantFarmMembershipCommand(executorUserId, farmId, targetUserId, FarmRole.Buyer);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.FarmMembershipId);
        Assert.NotNull(membershipRepository.AddedMembership);
        Assert.Equal(targetUserId, membershipRepository.AddedMembership!.UserId);
        Assert.Equal(FarmRole.Buyer, membershipRepository.AddedMembership!.Role);
        Assert.Equal(1, unitOfWork.BeginCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_ExecutorNotOwner_ThrowsForbidden()
    {
        var farmId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var farmRepository = new FakeFarmRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeFarmMembershipRepository
        {
            ExecutorMemberships = [FarmMembership.Grant(farmId, executorUserId, FarmRole.Buyer)]
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantFarmMembershipHandler(
            farmRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantFarmMembershipCommand(executorUserId, farmId, targetUserId, FarmRole.Buyer);

        await Assert.ThrowsAsync<FarmMembershipForbiddenException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_MembershipAlreadyExists_ThrowsConflict()
    {
        var farmId = Guid.NewGuid();
        var executorUserId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var farmRepository = new FakeFarmRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeFarmMembershipRepository
        {
            ExecutorMemberships = [FarmMembership.GrantOwner(farmId, executorUserId)],
            ExistsResult = true
        };
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantFarmMembershipHandler(
            farmRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantFarmMembershipCommand(executorUserId, farmId, targetUserId, FarmRole.Buyer);

        await Assert.ThrowsAsync<FarmMembershipAlreadyExistsException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_FarmNotFound_Throws()
    {
        var farmRepository = new FakeFarmRepository { ExistsResult = false };
        var userRepository = new FakeUserRepository { ExistsResult = true };
        var membershipRepository = new FakeFarmMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantFarmMembershipHandler(
            farmRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantFarmMembershipCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), FarmRole.Buyer);

        await Assert.ThrowsAsync<FarmNotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_TargetUserNotFound_Throws()
    {
        var farmId = Guid.NewGuid();

        var farmRepository = new FakeFarmRepository { ExistsResult = true };
        var userRepository = new FakeUserRepository { ExistsResult = false };
        var membershipRepository = new FakeFarmMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new GrantFarmMembershipHandler(
            farmRepository,
            userRepository,
            membershipRepository,
            unitOfWork);

        var command = new GrantFarmMembershipCommand(Guid.NewGuid(), farmId, Guid.NewGuid(), FarmRole.Buyer);

        await Assert.ThrowsAsync<UserNotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    private sealed class FakeFarmRepository : IFarmRepository
    {
        public bool ExistsResult { get; set; }

        public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ExistsAsync(Guid farmId, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task AddAsync(Agronomia.Domain.Organizations.Farm farm, CancellationToken ct)
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

    private sealed class FakeFarmMembershipRepository : IFarmMembershipRepository
    {
        public IReadOnlyList<FarmMembership> ExecutorMemberships { get; set; } = [];

        public bool ExistsResult { get; set; }

        public FarmMembership? AddedMembership { get; private set; }

        public Task<IReadOnlyList<FarmMembership>> GetByFarmAndUserAsync(
            Guid farmId,
            Guid userId,
            CancellationToken ct)
        {
            return Task.FromResult(ExecutorMemberships);
        }

        public Task<bool> ExistsAsync(Guid farmId, Guid userId, FarmRole role, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task AddAsync(FarmMembership membership, CancellationToken ct)
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
