using FluentValidation;

namespace Agronomia.Application.Features.Identity.RegisterUser;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
