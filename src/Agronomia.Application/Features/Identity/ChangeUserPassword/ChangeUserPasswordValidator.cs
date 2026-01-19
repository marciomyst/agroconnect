using FluentValidation;

namespace Agronomia.Application.Features.Identity.ChangeUserPassword;

public sealed class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.CurrentPassword)
            .NotEmpty();

        RuleFor(command => command.NewPassword)
            .NotEmpty()
            .MinimumLength(8);
    }
}
