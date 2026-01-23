using Agronomia.Domain.Common;

namespace Agronomia.Domain.Orders.PurchaseIntents;

public sealed class Quantity : ValueObject
{
    public decimal Value { get; private set; }

    private Quantity()
    {
    }

    private Quantity(decimal value)
    {
        Value = value;
    }

    public static Quantity Create(decimal value)
    {
        Guard.AgainstNegativeOrZero(value, nameof(value));
        return new Quantity(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("0.###");
}
