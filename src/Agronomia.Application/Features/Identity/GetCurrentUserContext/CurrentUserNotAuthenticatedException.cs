namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public sealed class CurrentUserNotAuthenticatedException()
    : InvalidOperationException("User is not authenticated.");
