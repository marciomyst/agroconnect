using Agronomia.Application.Features.Identity.AuthenticateUser;

namespace Agronomia.Api.Features.Auth.Authenticate;

public static class AuthenticateUserMapper
{
    public static AuthenticateUserCommand ToCommand(this AuthenticateUserRequest request)
    {
        return new AuthenticateUserCommand(request.Email, request.Password);
    }

    public static AuthenticateUserResponse FromResult(this AuthenticateUserResult result)
    {
        return new AuthenticateUserResponse(result.UserId, result.Email, result.AccessToken);
    }
}
