using Agronomia.Application.Features.Products.GetProductById;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Products.GetProductById;

public static class GetProductByIdEndpoint
{
    public static void MapGetProductById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var query = new GetProductByIdQuery(productId);

            try
            {
                var result = await messageBus.InvokeAsync<ProductDetailsDto?>(query, ct);
                return result is null
                    ? Results.Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Not Found",
                        detail: "Product not found.")
                    : Results.Ok(result);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("GetProductById")
        .WithTags("Products")
        .WithSummary("Get product details.")
        .WithDescription("Gets a product by identifier.")
        .RequireAuthorization("User")
        .Produces<ProductDetailsDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private static IDictionary<string, string[]> ToValidationErrors(ValidationException ex)
    {
        return ex.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
