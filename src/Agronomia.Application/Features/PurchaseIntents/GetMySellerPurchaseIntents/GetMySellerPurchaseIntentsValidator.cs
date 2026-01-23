using FluentValidation;

namespace Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;

public sealed class GetMySellerPurchaseIntentsValidator : AbstractValidator<GetMySellerPurchaseIntentsQuery>
{
    public GetMySellerPurchaseIntentsValidator()
    {
        RuleFor(query => query.SellerId)
            .NotEmpty();

        RuleFor(query => query.ExecutorUserId)
            .NotEmpty();
    }
}
