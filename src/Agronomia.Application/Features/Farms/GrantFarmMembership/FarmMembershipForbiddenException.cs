namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class FarmMembershipForbiddenException()
    : UnauthorizedAccessException("Only a Farm Owner can grant memberships.");
