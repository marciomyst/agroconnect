namespace Agronomia.Application.Features.Identity.RegisterUser;

public sealed class UserEmailAlreadyExistsException(string email)
    : InvalidOperationException($"Email '{email}' is already registered.")
{
    public string Email { get; } = email;
}
