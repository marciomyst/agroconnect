using Agronomia.Api.Features.Sellers.GrantSellerMembership;
using Agronomia.Api.Features.Sellers.RegisterSellerWithOwner;

namespace Agronomia.Api.Features.Sellers;

public static class SellersEndpoints
{
    public static void MapSellerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGrantSellerMembership();
        app.MapRegisterSellerWithOwner();
    }
}
