using Agronomia.Domain.Common;
using Agronomia.Domain.Memberships.Events;

namespace Agronomia.Domain.Memberships;

public sealed class FarmMembership : AggregateRoot
{
    private FarmMembership()
    {
    }

    private FarmMembership(Guid id, Guid farmId, Guid userId, FarmRole role)
        : base(id)
    {
        FarmId = farmId;
        UserId = userId;
        Role = role;
    }

    public Guid FarmId { get; private set; }

    public Guid UserId { get; private set; }

    public FarmRole Role { get; private set; }

    public static FarmMembership Grant(Guid farmId, Guid userId, FarmRole role, DateTime? nowUtc = null)
    {
        if (farmId == Guid.Empty)
        {
            throw new ArgumentException("FarmId is required.", nameof(farmId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (!Enum.IsDefined(typeof(FarmRole), role))
        {
            throw new ArgumentOutOfRangeException(nameof(role), "Role is invalid.");
        }

        var occurredAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);
        var membership = new FarmMembership(Guid.NewGuid(), farmId, userId, role);

        membership.AddDomainEvent(new FarmMembershipGranted(
            Guid.NewGuid(),
            occurredAtUtc,
            membership.FarmId,
            membership.UserId,
            membership.Role));

        return membership;
    }

    public static FarmMembership GrantOwner(Guid farmId, Guid userId, DateTime? nowUtc = null)
    {
        return Grant(farmId, userId, FarmRole.Owner, nowUtc);
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
