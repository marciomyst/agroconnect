using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Domain.Memberships;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfSellerMembershipRepository(AgronomiaDbContext context) : ISellerMembershipRepository
{
    public async Task<IReadOnlyList<SellerMembership>> GetBySellerAndUserAsync(
        Guid sellerId,
        Guid userId,
        CancellationToken ct)
    {
        return await context.SellerMemberships
            .AsNoTracking()
            .Where(membership => membership.SellerId == sellerId && membership.UserId == userId)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid sellerId, Guid userId, SellerRole role, CancellationToken ct)
    {
        return context.SellerMemberships
            .AsNoTracking()
            .AnyAsync(membership =>
                membership.SellerId == sellerId &&
                membership.UserId == userId &&
                membership.Role == role, ct);
    }

    public async Task AddAsync(SellerMembership membership, CancellationToken ct)
    {
        await context.SellerMemberships.AddAsync(membership, ct);
    }
}
