using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Identity.ChangeUserPassword;

public sealed record ChangeUserPasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : ICommand<ChangeUserPasswordResult>;
