namespace Agronomia.Domain.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);
    }

    public static void AgainstNullOrEmpty(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
    }

    public static void AgainstNegativeOrZero(decimal value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be greater than zero.");
    }
}
