using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Organizations;
using Xunit;

namespace Agronomia.Application.Tests.Sellers;

public sealed class RegisterSellerWithOwnerHandlerTests
{
    [Fact]
    public async Task HandleAsync_DuplicateTaxId_ThrowsConflict()
    {
        var sellerRepository = new FakeSellerRepository { ExistsResult = true };
        var membershipRepository = new FakeSellerMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterSellerWithOwnerHandler(sellerRepository, membershipRepository, unitOfWork);

        var command = new RegisterSellerWithOwnerCommand(Guid.NewGuid(), "123", "Acme Trading");

        await Assert.ThrowsAsync<SellerTaxIdAlreadyExistsException>(() => handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal(0, unitOfWork.BeginCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_PersistsAndCommits()
    {
        var sellerRepository = new FakeSellerRepository();
        var membershipRepository = new FakeSellerMembershipRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterSellerWithOwnerHandler(sellerRepository, membershipRepository, unitOfWork);

        var userId = Guid.NewGuid();
        var command = new RegisterSellerWithOwnerCommand(userId, "123", "Acme Trading");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotNull(sellerRepository.AddedSeller);
        Assert.NotNull(membershipRepository.AddedMembership);
        Assert.Equal(result.SellerId, sellerRepository.AddedSeller!.Id);
        Assert.Equal(result.SellerMembershipId, membershipRepository.AddedMembership!.Id);
        Assert.Equal(userId, membershipRepository.AddedMembership!.UserId);
        Assert.Equal(SellerRole.Owner, membershipRepository.AddedMembership!.Role);
        Assert.Equal(1, unitOfWork.BeginCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(0, unitOfWork.RollbackCalls);
    }

    private sealed class FakeSellerRepository : ISellerRepository
    {
        public bool ExistsResult { get; set; }

        public Seller? AddedSeller { get; private set; }

        public Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken ct)
        {
            return Task.FromResult(ExistsResult);
        }

        public Task<bool> ExistsAsync(Guid sellerId, CancellationToken ct)
        {
            return Task.FromResult(false);
        }

        public Task AddAsync(Seller seller, CancellationToken ct)
        {
            AddedSeller = seller;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeSellerMembershipRepository : ISellerMembershipRepository
    {
        public SellerMembership? AddedMembership { get; private set; }

        public Task<IReadOnlyList<SellerMembership>> GetBySellerAndUserAsync(
            Guid sellerId,
            Guid userId,
            CancellationToken ct)
        {
            return Task.FromResult<IReadOnlyList<SellerMembership>>(Array.Empty<SellerMembership>());
        }

        public Task<bool> ExistsAsync(Guid sellerId, Guid userId, SellerRole role, CancellationToken ct)
        {
            return Task.FromResult(false);
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
