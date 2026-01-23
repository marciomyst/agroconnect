using Agronomia.Domain.Common;

namespace Agronomia.Domain.Catalog.Products;

public sealed class RegistrationNumber : ValueObject
{
    public const int MaxLength = 64;

    public string Value { get; private set; } = string.Empty;

    private RegistrationNumber()
    {
    }

    private RegistrationNumber(string value)
    {
        Value = value;
    }

    public static RegistrationNumber Create(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Registration number cannot exceed {MaxLength} characters.");
        }

        return new RegistrationNumber(normalized);
    }

    public static RegistrationNumber? CreateOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : Create(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
