using System.Security.Cryptography;
using System.Text;

namespace Agronomia.Crosscutting.Security;

/// <summary>
/// Provides hashing helpers for password validation scenarios.
/// </summary>
public static class PasswordHasher
{
    public static bool Verify(string storedHash, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        try
        {
            ReadOnlySpan<byte> storedBytes = Convert.FromHexString(storedHash);
            ReadOnlySpan<byte> providedBytes = Convert.FromHexString(GenerateValidationHash(providedPassword));

            return CryptographicOperations.FixedTimeEquals(storedBytes, providedBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static string GenerateValidationHash(string password)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(hashBytes);
    }
}
