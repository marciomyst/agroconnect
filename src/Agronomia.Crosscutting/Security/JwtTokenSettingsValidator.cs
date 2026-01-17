using FluentValidation;

namespace Agronomia.Crosscutting.Security;

public class JwtTokenSettingsValidator : AbstractValidator<JwtTokenSettings>
{
    public JwtTokenSettingsValidator()
    {
        RuleFor(x => x.Secret)
            .NotEmpty()
            .MinimumLength(32).WithMessage("Secret must have 32 characters.");

        RuleFor(x => x.Issuer).NotEmpty();
        RuleFor(x => x.Audience).NotEmpty();
        RuleFor(x => x.ExpiresInMinutes).InclusiveBetween(1, 1440);
    }
}