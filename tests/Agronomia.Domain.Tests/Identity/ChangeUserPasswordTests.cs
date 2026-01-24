using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.Events;
using Agronomia.Domain.Identity.ValueObjects;
using Xunit;

namespace Agronomia.Domain.Tests.Identity;

public sealed class ChangeUserPasswordTests
{
    [Fact]
    public void ChangePassword_ValidHash_RaisesUserPasswordChanged()
    {
        var email = Email.Create("user@example.com");
        var user = User.Register("User", email, "old-hash");

        user.ChangePassword("new-hash");

        var domainEvent = Assert.Single(user.DomainEvents, e => e is UserPasswordChanged);
        var changed = Assert.IsType<UserPasswordChanged>(domainEvent);

        Assert.Equal(user.Id, changed.UserId);
        Assert.Equal(DateTimeKind.Utc, changed.OccurredAtUtc.Kind);
    }

    [Fact]
    public void ChangePassword_InvalidHash_Throws()
    {
        var email = Email.Create("user@example.com");
        var user = User.Register("User", email, "old-hash");

        Assert.Throws<ArgumentException>(() => user.ChangePassword(" "));
    }
}
