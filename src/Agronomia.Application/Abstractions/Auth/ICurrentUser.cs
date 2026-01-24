namespace Agronomia.Application.Abstractions.Auth;

/// <summary>
/// Exposes the authenticated user's identity from the current request context.
/// </summary>
public interface ICurrentUser
{
    Guid? UserId { get; }

    string? Email { get; }
}
