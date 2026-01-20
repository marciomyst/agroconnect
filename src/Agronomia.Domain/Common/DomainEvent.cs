namespace Agronomia.Domain.Common;

public abstract record DomainEvent(
    Guid EventId,
    DateTime OccurredAt
);
