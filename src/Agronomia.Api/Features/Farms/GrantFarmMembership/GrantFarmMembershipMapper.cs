using Agronomia.Application.Features.Farms.GrantFarmMembership;

namespace Agronomia.Api.Features.Farms.GrantFarmMembership;

public static class GrantFarmMembershipMapper
{
    public static GrantFarmMembershipCommand ToCommand(
        this GrantFarmMembershipHttpRequest request,
        Guid executorUserId,
        Guid farmId)
    {
        return new GrantFarmMembershipCommand(
            executorUserId,
            farmId,
            request.UserId,
            request.Role);
    }

    public static GrantFarmMembershipHttpResponse FromResult(this GrantFarmMembershipResult result)
    {
        return new GrantFarmMembershipHttpResponse(result.FarmMembershipId);
    }
}
