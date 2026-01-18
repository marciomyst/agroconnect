using Agronomia.Api.Features.Sellers.CreateSeller;

namespace Agronomia.Api.Features.Sellers;

/// <summary>
/// Maps seller-related endpoints.
/// </summary>
public static class SellerEndpoints
{
    public static void MapSellerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateSeller();
    }
}
