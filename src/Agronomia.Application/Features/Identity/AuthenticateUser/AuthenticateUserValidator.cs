using FluentValidation;

namespace Agronomia.Application.Features.Identity.AuthenticateUser;

public sealed class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
