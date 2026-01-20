using Agronomia.Domain.Common;
using Agronomia.Domain.Organizations.Events;

namespace Agronomia.Domain.Organizations;

public sealed class Farm : AggregateRoot
{
    private Farm()
    {
    }

    private Farm(Guid id, string taxId, string name)
        : base(id)
    {
        TaxId = taxId;
        Name = name;
    }

    public string TaxId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public static Farm Register(string taxId, string name, DateTime? nowUtc = null)
    {
        Guard.AgainstNullOrEmpty(taxId, nameof(taxId));
        Guard.AgainstNullOrEmpty(name, nameof(name));

        var normalizedTaxId = taxId.Trim();
        var normalizedName = name.Trim();
        var occurredAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);

        var farm = new Farm(Guid.NewGuid(), normalizedTaxId, normalizedName);
        farm.AddDomainEvent(new FarmRegistered(Guid.NewGuid(), occurredAtUtc, farm.Id, farm.TaxId));

        return farm;
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
