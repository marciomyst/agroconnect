using Agronomia.Api.Features.Farms.GrantFarmMembership;
using Agronomia.Api.Features.Farms.RegisterFarmWithOwner;

namespace Agronomia.Api.Features.Farms;

public static class FarmsEndpoints
{
    public static void MapFarmEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGrantFarmMembership();
        app.MapRegisterFarmWithOwner();
    }
}
