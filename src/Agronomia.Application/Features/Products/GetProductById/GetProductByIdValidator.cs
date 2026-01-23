using FluentValidation;

namespace Agronomia.Application.Features.Products.GetProductById;

public sealed class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(query => query.ProductId)
            .NotEmpty();
    }
}
