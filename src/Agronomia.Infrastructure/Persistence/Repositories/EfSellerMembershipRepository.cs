using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Domain.Memberships;

namespace Agronomia.Infrastructure.Persistence.Repositories;

public sealed class EfSellerMembershipRepository(AgronomiaDbContext context) : ISellerMembershipRepository
{
    public async Task AddAsync(SellerMembership membership, CancellationToken ct)
    {
        await context.SellerMemberships.AddAsync(membership, ct);
    }
}
