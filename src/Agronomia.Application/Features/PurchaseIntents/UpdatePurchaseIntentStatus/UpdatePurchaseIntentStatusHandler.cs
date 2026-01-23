using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Orders.PurchaseIntents;

namespace Agronomia.Application.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

public sealed class UpdatePurchaseIntentStatusHandler(
    ISellerMembershipRepository sellerMembershipRepository,
    IPurchaseIntentRepository purchaseIntentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdatePurchaseIntentStatusCommand, UpdatePurchaseIntentStatusResult>
{
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IPurchaseIntentRepository _purchaseIntentRepository = purchaseIntentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdatePurchaseIntentStatusResult> HandleAsync(
        UpdatePurchaseIntentStatusCommand command,
        CancellationToken ct)
    {
        var memberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            command.SellerId,
            command.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is SellerRole.Owner or SellerRole.Manager))
        {
            throw new PurchaseIntentForbiddenException();
        }

        var intent = await _purchaseIntentRepository.GetByIdAsync(command.PurchaseIntentId, ct);
        if (intent is null || intent.SellerId != command.SellerId)
        {
            throw new PurchaseIntentNotFoundException(command.PurchaseIntentId);
        }

        if (!Enum.TryParse<PurchaseIntentStatus>(command.Status, ignoreCase: true, out var status))
        {
            throw new InvalidPurchaseIntentStatusException(command.Status);
        }

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            intent.UpdateStatus(status);
            await _unitOfWork.CommitAsync(ct);
            return new UpdatePurchaseIntentStatusResult(intent.Id);
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw new PurchaseIntentStatusTransitionException(ex.Message);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
