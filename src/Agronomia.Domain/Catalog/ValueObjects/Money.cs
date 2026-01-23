using Agronomia.Domain.Common;

namespace Agronomia.Domain.Catalog.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }

    public Currency Currency { get; private set; }

    private Money()
    {
    }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        Guard.AgainstNegativeOrZero(amount, nameof(amount));

        if (!Enum.IsDefined(typeof(Currency), currency))
        {
            throw new ArgumentOutOfRangeException(nameof(currency), "Currency is invalid.");
        }

        return new Money(amount, currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Amount:0.##}";
}
