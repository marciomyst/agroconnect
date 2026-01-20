using Agronomia.Application.Features.Identity.RegisterUser;

namespace Agronomia.Api.Features.Users.RegisterUser;

public static class RegisterUserMapper
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request)
    {
        return new RegisterUserCommand(request.Name, request.Email, request.Password);
    }

    public static RegisterUserResponse FromResult(this RegisterUserResult result)
    {
        return new RegisterUserResponse(result.UserId, result.Email);
    }
}
