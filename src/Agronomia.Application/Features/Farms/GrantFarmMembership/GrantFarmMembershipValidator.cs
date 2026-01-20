using FluentValidation;

namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class GrantFarmMembershipValidator : AbstractValidator<GrantFarmMembershipCommand>
{
    public GrantFarmMembershipValidator()
    {
        RuleFor(command => command.ExecutorUserId)
            .NotEmpty();

        RuleFor(command => command.FarmId)
            .NotEmpty();

        RuleFor(command => command.TargetUserId)
            .NotEmpty();

        RuleFor(command => command.Role)
            .IsInEnum();
    }
}
