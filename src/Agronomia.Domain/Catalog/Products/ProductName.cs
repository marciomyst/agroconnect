using Agronomia.Domain.Common;

namespace Agronomia.Domain.Catalog.Products;

public sealed class ProductName : ValueObject
{
    public const int MaxLength = 256;

    public string Value { get; private set; } = string.Empty;

    private ProductName()
    {
    }

    private ProductName(string value)
    {
        Value = value;
    }

    public static ProductName Create(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Product name cannot exceed {MaxLength} characters.");
        }

        return new ProductName(normalized);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
