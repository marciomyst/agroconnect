using Agronomia.Domain.Common;

namespace Agronomia.Domain.Identity.Events;

public sealed record UserAuthenticated(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid UserId,
    DateTime AuthenticatedAtUtc
) : DomainEvent(EventId, OccurredAtUtc);
