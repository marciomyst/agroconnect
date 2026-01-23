using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Orders.PurchaseIntents;

namespace Agronomia.Application.Features.PurchaseIntents.CreatePurchaseIntent;

public sealed class CreatePurchaseIntentHandler(
    IFarmMembershipRepository farmMembershipRepository,
    ISellerProductRepository sellerProductRepository,
    IPurchaseIntentRepository purchaseIntentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePurchaseIntentCommand, CreatePurchaseIntentResult>
{
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly IPurchaseIntentRepository _purchaseIntentRepository = purchaseIntentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreatePurchaseIntentResult> HandleAsync(CreatePurchaseIntentCommand command, CancellationToken ct)
    {
        var memberships = await _farmMembershipRepository.GetByFarmAndUserAsync(
            command.FarmId,
            command.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is FarmRole.Owner or FarmRole.Buyer))
        {
            throw new PurchaseIntentForbiddenException();
        }

        var sellerProduct = await _sellerProductRepository.GetByIdAsync(command.SellerProductId, ct);
        if (sellerProduct is null)
        {
            throw new SellerProductNotFoundException(command.SellerProductId);
        }

        if (!sellerProduct.IsAvailable)
        {
            throw new SellerProductNotAvailableException(command.SellerProductId);
        }

        var quantity = Quantity.Create(command.Quantity);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var intent = PurchaseIntent.Create(
                command.FarmId,
                sellerProduct.SellerId,
                sellerProduct.ProductId,
                sellerProduct.Id,
                quantity,
                command.Notes);

            await _purchaseIntentRepository.AddAsync(intent, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreatePurchaseIntentResult(intent.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
