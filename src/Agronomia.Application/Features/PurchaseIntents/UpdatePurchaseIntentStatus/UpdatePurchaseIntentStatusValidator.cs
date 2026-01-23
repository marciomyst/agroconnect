using FluentValidation;

namespace Agronomia.Application.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

public sealed class UpdatePurchaseIntentStatusValidator : AbstractValidator<UpdatePurchaseIntentStatusCommand>
{
    public UpdatePurchaseIntentStatusValidator()
    {
        RuleFor(command => command.SellerId)
            .NotEmpty();

        RuleFor(command => command.ExecutorUserId)
            .NotEmpty();

        RuleFor(command => command.PurchaseIntentId)
            .NotEmpty();

        RuleFor(command => command.Status)
            .NotEmpty();
    }
}
