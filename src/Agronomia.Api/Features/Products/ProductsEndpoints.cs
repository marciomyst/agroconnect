using Agronomia.Api.Features.Products.CreateProduct;
using Agronomia.Api.Features.Products.GetProductById;
using Agronomia.Api.Features.Products.SearchProducts;

namespace Agronomia.Api.Features.Products;

public static class ProductsEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateProduct();
        app.MapGetProductById();
        app.MapSearchProducts();
    }
}
