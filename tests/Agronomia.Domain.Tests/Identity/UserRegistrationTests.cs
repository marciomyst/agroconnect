using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.Events;
using Agronomia.Domain.Identity.ValueObjects;
using Xunit;

namespace Agronomia.Domain.Tests.Identity;

public sealed class UserRegistrationTests
{
    [Fact]
    public void Register_ValidUser_RaisesUserRegistered()
    {
        var email = Email.Create("User@Example.com");

        var user = User.Register("Test User", email, "hashed-password");

        var domainEvent = Assert.Single(user.DomainEvents);
        var registered = Assert.IsType<UserRegistered>(domainEvent);

        Assert.Equal(user.Id, registered.UserId);
        Assert.Equal(email.Value, registered.Email);
        Assert.Equal(DateTimeKind.Utc, registered.OccurredAtUtc.Kind);
    }

    [Fact]
    public void Register_InvalidName_Throws()
    {
        var email = Email.Create("user@example.com");

        Assert.Throws<ArgumentException>(() => User.Register(" ", email, "hashed-password"));
    }

    [Fact]
    public void Email_InvalidFormat_Throws()
    {
        Assert.Throws<ArgumentException>(() => Email.Create("invalid-email"));
    }

    [Fact]
    public void Register_InvalidPasswordHash_Throws()
    {
        var email = Email.Create("user@example.com");

        Assert.Throws<ArgumentException>(() => User.Register("User", email, ""));
    }
}
