using Agronomia.Domain.Common;

namespace Agronomia.Domain.Memberships.Events;

public sealed record FarmMembershipGranted(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid FarmId,
    Guid UserId,
    FarmRole Role
) : DomainEvent(EventId, OccurredAtUtc);
