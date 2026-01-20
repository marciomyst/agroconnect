using System.Net.Mail;
using Agronomia.Domain.Common;

namespace Agronomia.Domain.Identity.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; private set; } = string.Empty;

    private Email()
    {
    }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        var normalized = value.Trim().ToLowerInvariant();
        if (!IsValid(normalized))
        {
            throw new ArgumentException("Email is invalid.", nameof(value));
        }

        return new Email(normalized);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    private static bool IsValid(string value)
    {
        try
        {
            _ = new MailAddress(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
