namespace Agronomia.Application.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email);
}
