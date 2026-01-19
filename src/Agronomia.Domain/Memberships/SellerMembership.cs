using Agronomia.Domain.Common;
using Agronomia.Domain.Memberships.Events;

namespace Agronomia.Domain.Memberships;

public sealed class SellerMembership : AggregateRoot
{
    private SellerMembership()
    {
    }

    private SellerMembership(Guid id, Guid sellerId, Guid userId, SellerRole role)
        : base(id)
    {
        SellerId = sellerId;
        UserId = userId;
        Role = role;
    }

    public Guid SellerId { get; private set; }

    public Guid UserId { get; private set; }

    public SellerRole Role { get; private set; }

    public static SellerMembership GrantOwner(Guid sellerId, Guid userId, DateTime? nowUtc = null)
    {
        if (sellerId == Guid.Empty)
        {
            throw new ArgumentException("SellerId is required.", nameof(sellerId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        var occurredAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);
        var membership = new SellerMembership(Guid.NewGuid(), sellerId, userId, SellerRole.Owner);

        membership.AddDomainEvent(new SellerMembershipGranted(
            Guid.NewGuid(),
            occurredAtUtc,
            membership.SellerId,
            membership.UserId,
            membership.Role));

        return membership;
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
