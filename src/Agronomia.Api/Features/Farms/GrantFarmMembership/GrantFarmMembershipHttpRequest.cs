using Agronomia.Domain.Memberships;

namespace Agronomia.Api.Features.Farms.GrantFarmMembership;

public sealed record GrantFarmMembershipHttpRequest(
    Guid UserId,
    FarmRole Role
);
