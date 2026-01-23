using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Application.Features.Sellers.GrantSellerMembership;
using Agronomia.Domain.Catalog.ValueObjects;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.SellerCatalog.UpdateSellerProduct;

public sealed class UpdateSellerProductHandler(
    ISellerRepository sellerRepository,
    ISellerProductRepository sellerProductRepository,
    ISellerMembershipRepository sellerMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateSellerProductCommand, UpdateSellerProductResult>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateSellerProductResult> HandleAsync(UpdateSellerProductCommand command, CancellationToken ct)
    {
        if (!await _sellerRepository.ExistsAsync(command.SellerId, ct))
        {
            throw new SellerNotFoundException(command.SellerId);
        }

        var memberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            command.SellerId,
            command.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is SellerRole.Owner or SellerRole.Manager))
        {
            throw new SellerCatalogForbiddenException();
        }

        var sellerProduct = await _sellerProductRepository.GetByIdAsync(command.SellerProductId, ct);
        if (sellerProduct is null || sellerProduct.SellerId != command.SellerId)
        {
            throw new SellerProductNotFoundException(command.SellerProductId);
        }

        var currency = Enum.Parse<Currency>(command.Currency, ignoreCase: true);
        var price = Money.Create(command.Price, currency);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            sellerProduct.Update(price, command.IsAvailable);
            await _unitOfWork.CommitAsync(ct);
            return new UpdateSellerProductResult(sellerProduct.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
