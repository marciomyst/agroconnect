namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public sealed class CurrentUserNotFoundException(Guid userId)
    : InvalidOperationException($"User '{userId}' was not found.")
{
    public Guid UserId { get; } = userId;
}
