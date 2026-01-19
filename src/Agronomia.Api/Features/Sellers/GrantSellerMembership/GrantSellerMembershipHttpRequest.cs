using Agronomia.Domain.Memberships;

namespace Agronomia.Api.Features.Sellers.GrantSellerMembership;

public sealed record GrantSellerMembershipHttpRequest(
    Guid UserId,
    SellerRole Role
);
