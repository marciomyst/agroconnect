using Agronomia.Domain.Common;
using Agronomia.Domain.Organizations.Events;

namespace Agronomia.Domain.Organizations;

public sealed class Seller : AggregateRoot
{
    private Seller()
    {
    }

    private Seller(Guid id, string taxId, string corporateName)
        : base(id)
    {
        TaxId = taxId;
        CorporateName = corporateName;
    }

    public string TaxId { get; private set; } = string.Empty;

    public string CorporateName { get; private set; } = string.Empty;

    public static Seller Register(string taxId, string corporateName, DateTime? nowUtc = null)
    {
        Guard.AgainstNullOrEmpty(taxId, nameof(taxId));
        Guard.AgainstNullOrEmpty(corporateName, nameof(corporateName));

        var normalizedTaxId = taxId.Trim();
        var normalizedCorporateName = corporateName.Trim();
        var occurredAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);

        var seller = new Seller(Guid.NewGuid(), normalizedTaxId, normalizedCorporateName);
        seller.AddDomainEvent(new SellerRegistered(Guid.NewGuid(), occurredAtUtc, seller.Id, seller.TaxId, seller.CorporateName));

        return seller;
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
