using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Domain.Memberships;
using Agronomia.Domain.Organizations;

namespace Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

public sealed class RegisterFarmWithOwnerHandler(
    IFarmRepository farmRepository,
    IFarmMembershipRepository farmMembershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterFarmWithOwnerCommand, RegisterFarmWithOwnerResult>
{
    private readonly IFarmRepository _farmRepository = farmRepository;
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisterFarmWithOwnerResult> HandleAsync(RegisterFarmWithOwnerCommand command, CancellationToken ct)
    {
        var taxId = command.TaxId.Trim();
        var name = command.Name.Trim();

        if (await _farmRepository.ExistsByTaxIdAsync(taxId, ct))
        {
            throw new FarmTaxIdAlreadyExistsException(taxId);
        }

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var farm = Farm.Register(taxId, name);
            var membership = FarmMembership.GrantOwner(farm.Id, command.UserId);

            await _farmRepository.AddAsync(farm, ct);
            await _farmMembershipRepository.AddAsync(membership, ct);

            await _unitOfWork.CommitAsync(ct);

            return new RegisterFarmWithOwnerResult(farm.Id, membership.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
