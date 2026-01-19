using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Organizations;

namespace Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

public sealed class RegisterSellerWithOwnerHandler(
    ISellerRepository sellerRepository,
    ISellerMembershipRepository sellerMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterSellerWithOwnerCommand, RegisterSellerWithOwnerResult>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisterSellerWithOwnerResult> HandleAsync(RegisterSellerWithOwnerCommand command, CancellationToken ct)
    {
        var taxId = command.TaxId.Trim();
        var corporateName = command.CorporateName.Trim();

        if (await _sellerRepository.ExistsByTaxIdAsync(taxId, ct))
        {
            throw new SellerTaxIdAlreadyExistsException(taxId);
        }

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var seller = Seller.Register(taxId, corporateName);
            var membership = SellerMembership.GrantOwner(seller.Id, command.UserId);

            await _sellerRepository.AddAsync(seller, ct);
            await _sellerMembershipRepository.AddAsync(membership, ct);

            await _unitOfWork.CommitAsync(ct);

            return new RegisterSellerWithOwnerResult(seller.Id, membership.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
