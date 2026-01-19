using Agronomia.Domain.Common;

namespace Agronomia.Domain.Organizations.Events;

public sealed record SellerRegistered(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid SellerId,
    string TaxId,
    string CorporateName
) : DomainEvent(EventId, OccurredAtUtc);
