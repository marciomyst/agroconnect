using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class GrantSellerMembershipHandler(
    ISellerRepository sellerRepository,
    IUserRepository userRepository,
    ISellerMembershipRepository sellerMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<GrantSellerMembershipCommand, GrantSellerMembershipResult>
{
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GrantSellerMembershipResult> HandleAsync(GrantSellerMembershipCommand command, CancellationToken ct)
    {
        if (!await _sellerRepository.ExistsAsync(command.SellerId, ct))
        {
            throw new SellerNotFoundException(command.SellerId);
        }

        if (!await _userRepository.ExistsAsync(command.TargetUserId, ct))
        {
            throw new UserNotFoundException(command.TargetUserId);
        }

        var executorMemberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            command.SellerId,
            command.ExecutorUserId,
            ct);

        if (!executorMemberships.Any(membership => membership.Role == SellerRole.Owner))
        {
            throw new SellerMembershipForbiddenException();
        }

        if (await _sellerMembershipRepository.ExistsAsync(command.SellerId, command.TargetUserId, command.Role, ct))
        {
            throw new SellerMembershipAlreadyExistsException(command.SellerId, command.TargetUserId, command.Role.ToString());
        }

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var membership = SellerMembership.Grant(command.SellerId, command.TargetUserId, command.Role);
            await _sellerMembershipRepository.AddAsync(membership, ct);

            await _unitOfWork.CommitAsync(ct);

            return new GrantSellerMembershipResult(membership.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
