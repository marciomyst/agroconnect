using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Abstractions.Memberships;

public interface IFarmMembershipRepository
{
    Task<IReadOnlyList<FarmMembership>> GetByFarmAndUserAsync(Guid farmId, Guid userId, CancellationToken ct);

    Task<bool> ExistsAsync(Guid farmId, Guid userId, FarmRole role, CancellationToken ct);

    Task AddAsync(FarmMembership membership, CancellationToken ct);
}
