using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Features.Farms.RegisterFarmWithOwner;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Organizations;
using Xunit;

namespace Agronomia.Application.Tests.Farms;

public sealed class RegisterFarmWithOwnerHandlerTests
{
    [Fact]
    public async Task HandleAsync_DuplicateTaxId_ThrowsConflict()
    {
        var farmRepository = new FakeFarmRepository { ExistsResult = true };
        var membershipRepository = new FakeFarmMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterFarmWithOwnerHandler(farmRepository, membershipRepository, unitOfWork);

        var command = new RegisterFarmWithOwnerCommand(Guid.NewGuid(), "123", "Green Valley");

        await Assert.ThrowsAsync<FarmTaxIdAlreadyExistsException>(() => handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_PersistsAndCommits()
    {
        var farmRepository = new FakeFarmRepository();
        var membershipRepository = new FakeFarmMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterFarmWithOwnerHandler(farmRepository, membershipRepository, unitOfWork);

        var userId = Guid.NewGuid();
        var command = new RegisterFarmWithOwnerCommand(userId, "123", "Green Valley");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotNull(farmRepository.AddedFarm);
        Assert.NotNull(membershipRepository.AddedMembership);
        Assert.Equal(result.FarmId, farmRepository.AddedFarm!.Id);
        Assert.Equal(result.FarmMembershipId, membershipRepository.AddedMembership!.Id);
        Assert.Equal(userId, membershipRepository.AddedMembership!.UserId);
        Assert.Equal(FarmRole.Owner, membershipRepository.AddedMembership!.Role);
        Assert.Equal(1, unitOfWork.BeginCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    private sealed class FakeFarmRepository : IFarmRepository
    {
        public bool ExistsResult { get; set; }

        public Farm? AddedFarm { get; private set; }

        public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task<bool> ExistsAsync(Guid farmId, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task AddAsync(Farm farm, CancellationToken ct)
        {
            AddedFarm = farm;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeFarmMembershipRepository : IFarmMembershipRepository
    {
        public FarmMembership? AddedMembership { get; private set; }

        public Task<IReadOnlyList<FarmMembership>> GetByFarmAndUserAsync(
            Guid farmId,
            Guid userId,
            CancellationToken ct)
        {
            return Task.FromResult<IReadOnlyList<FarmMembership>>(Array.Empty<FarmMembership>());
        }

        public Task<bool> ExistsAsync(Guid farmId, Guid userId, FarmRole role, CancellationToken ct)
        {
            return Task.FromResult(false);
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
