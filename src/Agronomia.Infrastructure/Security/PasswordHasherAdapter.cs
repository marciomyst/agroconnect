using Agronomia.Application.Abstractions.Security;
using Agronomia.Crosscutting.Security;

namespace Agronomia.Infrastructure.Security;

public sealed class PasswordHasherAdapter : IPasswordHasher
{
    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        return PasswordHasher.GenerateValidationHash(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        return PasswordHasher.Verify(passwordHash, password);
    }
}
