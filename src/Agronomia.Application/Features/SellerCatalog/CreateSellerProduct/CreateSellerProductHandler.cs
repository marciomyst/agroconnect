using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Catalog.SellerProducts;
using Agronomia.Domain.Catalog.ValueObjects;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.SellerCatalog.CreateSellerProduct;

public sealed class CreateSellerProductHandler(
    ISellerRepository sellerRepository,
    IProductRepository productRepository,
    ISellerProductRepository sellerProductRepository,
    ISellerMembershipRepository sellerMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSellerProductCommand, CreateSellerProductResult>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateSellerProductResult> HandleAsync(CreateSellerProductCommand command, CancellationToken ct)
    {
        if (!await _sellerRepository.ExistsAsync(command.SellerId, ct))
        {
            throw new SellerNotFoundException(command.SellerId);
        }

        if (!await _productRepository.ExistsAsync(command.ProductId, ct))
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        var memberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            command.SellerId,
            command.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is SellerRole.Owner or SellerRole.Manager))
        {
            throw new SellerCatalogForbiddenException();
        }

        if (await _sellerProductRepository.ExistsAsync(command.SellerId, command.ProductId, ct))
        {
            throw new SellerProductAlreadyExistsException(command.SellerId, command.ProductId);
        }

        var currency = Enum.Parse<Currency>(command.Currency, ignoreCase: true);
        var price = Money.Create(command.Price, currency);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var sellerProduct = SellerProduct.Create(
                command.SellerId,
                command.ProductId,
                price,
                command.IsAvailable);

            await _sellerProductRepository.AddAsync(sellerProduct, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreateSellerProductResult(sellerProduct.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
