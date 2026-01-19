using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Domain.Memberships;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfFarmMembershipRepository(AgronomiaDbContext context) : IFarmMembershipRepository
{
    public async Task<IReadOnlyList<FarmMembership>> GetByFarmAndUserAsync(
        Guid farmId,
        Guid userId,
        CancellationToken ct)
    {
        return await context.FarmMemberships
            .AsNoTracking()
            .Where(membership => membership.FarmId == farmId && membership.UserId == userId)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid farmId, Guid userId, FarmRole role, CancellationToken ct)
    {
        return context.FarmMemberships
            .AsNoTracking()
            .AnyAsync(membership =>
                membership.FarmId == farmId &&
                membership.UserId == userId &&
                membership.Role == role, ct);
    }

    public async Task AddAsync(FarmMembership membership, CancellationToken ct)
    {
        await context.FarmMemberships.AddAsync(membership, ct);
    }
}
