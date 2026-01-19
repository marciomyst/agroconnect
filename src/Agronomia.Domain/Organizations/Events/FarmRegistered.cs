using Agronomia.Domain.Common;

namespace Agronomia.Domain.Organizations.Events;

public sealed record FarmRegistered(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid FarmId,
    string TaxId
) : DomainEvent(EventId, OccurredAtUtc);
