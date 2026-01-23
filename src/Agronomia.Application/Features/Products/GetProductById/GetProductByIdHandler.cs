using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Products.GetProductById;

public sealed class GetProductByIdHandler(IProductReadRepository readRepository)
    : IQueryHandler<GetProductByIdQuery, ProductDetailsDto?>
{
    private readonly IProductReadRepository _readRepository = readRepository;

    public Task<ProductDetailsDto?> HandleAsync(GetProductByIdQuery query, CancellationToken ct)
        => _readRepository.GetByIdAsync(query.ProductId, ct);
}
