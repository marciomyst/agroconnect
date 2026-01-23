using FluentValidation;

namespace Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;

public sealed class GetMyFarmPurchaseIntentsValidator : AbstractValidator<GetMyFarmPurchaseIntentsQuery>
{
    public GetMyFarmPurchaseIntentsValidator()
    {
        RuleFor(query => query.FarmId)
            .NotEmpty();

        RuleFor(query => query.ExecutorUserId)
            .NotEmpty();
    }
}
