using Agronomia.Domain.Common;

namespace Agronomia.Domain.Identity.Events;

public sealed record UserPasswordChanged(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid UserId
) : DomainEvent(EventId, OccurredAtUtc);
