using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class GrantFarmMembershipHandler(
    IFarmRepository farmRepository,
    IUserRepository userRepository,
    IFarmMembershipRepository farmMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<GrantFarmMembershipCommand, GrantFarmMembershipResult>
{
    private readonly IFarmRepository _farmRepository = farmRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GrantFarmMembershipResult> HandleAsync(GrantFarmMembershipCommand command, CancellationToken ct)
    {
        if (!await _farmRepository.ExistsAsync(command.FarmId, ct))
        {
            throw new FarmNotFoundException(command.FarmId);
        }

        if (!await _userRepository.ExistsAsync(command.TargetUserId, ct))
        {
            throw new UserNotFoundException(command.TargetUserId);
        }

        var executorMemberships = await _farmMembershipRepository.GetByFarmAndUserAsync(
            command.FarmId,
            command.ExecutorUserId,
            ct);

        if (!executorMemberships.Any(membership => membership.Role == FarmRole.Owner))
        {
            throw new FarmMembershipForbiddenException();
        }

        if (await _farmMembershipRepository.ExistsAsync(command.FarmId, command.TargetUserId, command.Role, ct))
        {
            throw new FarmMembershipAlreadyExistsException(command.FarmId, command.TargetUserId, command.Role.ToString());
        }

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var membership = FarmMembership.Grant(command.FarmId, command.TargetUserId, command.Role);
            await _farmMembershipRepository.AddAsync(membership, ct);

            await _unitOfWork.CommitAsync(ct);

            return new GrantFarmMembershipResult(membership.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
