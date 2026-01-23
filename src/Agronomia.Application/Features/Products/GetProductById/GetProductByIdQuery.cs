using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDetailsDto?>;
