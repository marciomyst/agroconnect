using Agronomia.Domain.Memberships;
using Agronomia.Domain.Memberships.Events;
using Xunit;

namespace Agronomia.Domain.Tests.Memberships;

public sealed class SellerMembershipTests
{
    [Fact]
    public void GrantOwner_ValidMembership_RaisesSellerMembershipGranted()
    {
        var sellerId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var membership = SellerMembership.GrantOwner(sellerId, userId);

        var domainEvent = Assert.Single(membership.DomainEvents);
        var granted = Assert.IsType<SellerMembershipGranted>(domainEvent);

        Assert.Equal(sellerId, granted.SellerId);
        Assert.Equal(userId, granted.UserId);
        Assert.Equal(SellerRole.Owner, granted.Role);
        Assert.Equal(DateTimeKind.Utc, granted.OccurredAtUtc.Kind);
    }

    [Fact]
    public void GrantOwner_EmptySellerId_Throws()
    {
        Assert.Throws<ArgumentException>(() => SellerMembership.GrantOwner(Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void GrantOwner_EmptyUserId_Throws()
    {
        Assert.Throws<ArgumentException>(() => SellerMembership.GrantOwner(Guid.NewGuid(), Guid.Empty));
    }
}
