using Agronomia.Domain.Memberships;
using Agronomia.Domain.Memberships.Events;
using Xunit;

namespace Agronomia.Domain.Tests.Memberships;

public sealed class FarmMembershipTests
{
    [Fact]
    public void GrantOwner_ValidMembership_RaisesFarmMembershipGranted()
    {
        var farmId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var membership = FarmMembership.GrantOwner(farmId, userId);

        var domainEvent = Assert.Single(membership.DomainEvents);
        var granted = Assert.IsType<FarmMembershipGranted>(domainEvent);

        Assert.Equal(farmId, granted.FarmId);
        Assert.Equal(userId, granted.UserId);
        Assert.Equal(FarmRole.Owner, granted.Role);
        Assert.Equal(DateTimeKind.Utc, granted.OccurredAtUtc.Kind);
    }

    [Fact]
    public void Grant_ValidRole_RaisesFarmMembershipGranted()
    {
        var farmId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var membership = FarmMembership.Grant(farmId, userId, FarmRole.Buyer);

        var domainEvent = Assert.Single(membership.DomainEvents);
        var granted = Assert.IsType<FarmMembershipGranted>(domainEvent);

        Assert.Equal(farmId, granted.FarmId);
        Assert.Equal(userId, granted.UserId);
        Assert.Equal(FarmRole.Buyer, granted.Role);
        Assert.Equal(DateTimeKind.Utc, granted.OccurredAtUtc.Kind);
    }

    [Fact]
    public void GrantOwner_EmptyFarmId_Throws()
    {
        Assert.Throws<ArgumentException>(() => FarmMembership.GrantOwner(Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void GrantOwner_EmptyUserId_Throws()
    {
        Assert.Throws<ArgumentException>(() => FarmMembership.GrantOwner(Guid.NewGuid(), Guid.Empty));
    }
}
