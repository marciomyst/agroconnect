using Agronomia.Application.Features.Identity.ChangeUserPassword;

namespace Agronomia.Api.Features.Users.ChangeUserPassword;

public static class ChangeUserPasswordMapper
{
    public static ChangeUserPasswordCommand ToCommand(this ChangeUserPasswordRequest request, Guid userId)
    {
        return new ChangeUserPasswordCommand(userId, request.CurrentPassword, request.NewPassword);
    }

    public static ChangeUserPasswordResponse FromResult(this ChangeUserPasswordResult result)
    {
        return new ChangeUserPasswordResponse(result.UserId);
    }
}
