using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Abstractions.Memberships;

public interface ISellerMembershipRepository
{
    Task<IReadOnlyList<SellerMembership>> GetBySellerAndUserAsync(Guid sellerId, Guid userId, CancellationToken ct);

    Task<bool> ExistsAsync(Guid sellerId, Guid userId, SellerRole role, CancellationToken ct);

    Task AddAsync(SellerMembership membership, CancellationToken ct);
}
