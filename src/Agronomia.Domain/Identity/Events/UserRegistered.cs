using Agronomia.Domain.Common;

namespace Agronomia.Domain.Identity.Events;

public sealed record UserRegistered(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid UserId,
    string Email
) : DomainEvent(EventId, OccurredAtUtc);
