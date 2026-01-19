using Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

namespace Agronomia.Api.Features.Farms.RegisterFarmWithOwner;

public static class RegisterFarmWithOwnerMapper
{
    public static RegisterFarmWithOwnerCommand ToCommand(this RegisterFarmWithOwnerHttpRequest request, Guid userId)
    {
        return new RegisterFarmWithOwnerCommand(userId, request.TaxId, request.Name);
    }

    public static RegisterFarmWithOwnerHttpResponse FromResult(this RegisterFarmWithOwnerResult result)
    {
        return new RegisterFarmWithOwnerHttpResponse(result.FarmId, result.FarmMembershipId);
    }
}
