using FluentValidation;

namespace Agronomia.Application.Features.PurchaseIntents.CreatePurchaseIntent;

public sealed class CreatePurchaseIntentValidator : AbstractValidator<CreatePurchaseIntentCommand>
{
    public CreatePurchaseIntentValidator()
    {
        RuleFor(command => command.FarmId)
            .NotEmpty();

        RuleFor(command => command.ExecutorUserId)
            .NotEmpty();

        RuleFor(command => command.SellerProductId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0);

        RuleFor(command => command.Notes)
            .MaximumLength(1000)
            .When(command => !string.IsNullOrWhiteSpace(command.Notes));
    }
}
