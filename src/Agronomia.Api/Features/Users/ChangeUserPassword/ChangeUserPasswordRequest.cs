namespace Agronomia.Api.Features.Users.ChangeUserPassword;

public sealed record ChangeUserPasswordRequest(
    string CurrentPassword,
    string NewPassword
);
