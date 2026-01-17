using FluentValidation;

namespace Agronomia.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Validates <see cref="GetCurrentUserQuery"/> requests.
/// </summary>
internal sealed class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    public GetCurrentUserQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}
