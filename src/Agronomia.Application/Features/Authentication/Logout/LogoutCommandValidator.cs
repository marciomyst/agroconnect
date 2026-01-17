using FluentValidation;

namespace Agronomia.Application.Features.Authentication.Logout;

/// <summary>
/// Validates logout commands.
/// </summary>
public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(cmd => cmd.RefreshToken)
            .NotEmpty();
    }
}
