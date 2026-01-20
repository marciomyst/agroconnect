namespace Agronomia.Application.Features.Identity.ChangeUserPassword;

public sealed class InvalidCurrentPasswordException()
    : UnauthorizedAccessException("Invalid credentials.");
