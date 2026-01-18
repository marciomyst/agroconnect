using Agronomia.Domain.Common;
using Agronomia.Domain.Identity.Events;
using Agronomia.Domain.Identity.ValueObjects;

namespace Agronomia.Domain.Identity;

public sealed class User : AggregateRoot
{
    private User()
    {
    }

    private User(Guid id, string name, Email email, string passwordHash, DateTime createdAtUtc, bool isActive)
        : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = createdAtUtc;
        IsActive = isActive;
    }

    public string Name { get; private set; } = string.Empty;

    public Email Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public static User Register(string name, Email email, string passwordHash, DateTime? nowUtc = null)
    {
        Guard.AgainstNullOrEmpty(name, nameof(name));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNullOrEmpty(passwordHash, nameof(passwordHash));

        var trimmedName = name.Trim();
        var createdAt = NormalizeUtc(nowUtc ?? DateTime.UtcNow);

        var user = new User(Guid.NewGuid(), trimmedName, email, passwordHash, createdAt, isActive: true);
        user.AddDomainEvent(new UserRegistered(Guid.NewGuid(), createdAt, user.Id, user.Email.Value));

        return user;
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
