using FluentValidation;

namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class GrantSellerMembershipValidator : AbstractValidator<GrantSellerMembershipCommand>
{
    public GrantSellerMembershipValidator()
    {
        RuleFor(command => command.ExecutorUserId)
            .NotEmpty();

        RuleFor(command => command.SellerId)
            .NotEmpty();

        RuleFor(command => command.TargetUserId)
            .NotEmpty();

        RuleFor(command => command.Role)
            .IsInEnum();
    }
}
