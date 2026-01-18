using FluentValidation;

namespace Agronomia.Application.Features.Authentication.Refresh;

/// <summary>
/// Validates refresh token commands.
/// </summary>
public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(cmd => cmd.RefreshToken)
            .NotEmpty();

        RuleFor(cmd => cmd.DeviceId)
            .NotEmpty();
    }
}
