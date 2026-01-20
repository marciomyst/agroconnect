namespace Agronomia.Application.Features.Identity.AuthenticateUser;

public sealed class InvalidCredentialsException()
    : UnauthorizedAccessException("Invalid email or password.");
