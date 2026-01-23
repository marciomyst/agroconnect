using Agronomia.Api.Features.Sellers.Catalog.CreateSellerProduct;
using Agronomia.Api.Features.Sellers.Catalog.GetSellerCatalog;
using Agronomia.Api.Features.Sellers.Catalog.UpdateSellerProduct;

namespace Agronomia.Api.Features.Sellers.Catalog;

public static class SellerCatalogEndpoints
{
    public static void MapSellerCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetSellerCatalog();
        app.MapCreateSellerProduct();
        app.MapUpdateSellerProduct();
    }
}
