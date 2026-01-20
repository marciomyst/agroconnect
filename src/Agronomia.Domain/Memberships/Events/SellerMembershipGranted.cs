using Agronomia.Domain.Common;

namespace Agronomia.Domain.Memberships.Events;

public sealed record SellerMembershipGranted(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid SellerId,
    Guid UserId,
    SellerRole Role
) : DomainEvent(EventId, OccurredAtUtc);
