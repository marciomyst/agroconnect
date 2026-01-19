using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Abstractions.Memberships;

public interface ISellerMembershipRepository
{
    Task AddAsync(SellerMembership membership, CancellationToken ct);
}
