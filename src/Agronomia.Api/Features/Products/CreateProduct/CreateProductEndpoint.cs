using Agronomia.Application.Features.Products.CreateProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapCreateProduct(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", async (
            [FromBody] CreateProductHttpRequest request,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var command = request.ToCommand();

            try
            {
                var result = await messageBus.InvokeAsync<CreateProductResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/products/{response.ProductId}", response);
            }
            catch (ProductAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Product already exists.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("CreateProduct")
        .WithTags("Products")
        .WithSummary("Create a new product.")
        .WithDescription("Creates a global product in the catalog.")
        .RequireAuthorization("Administrator")
        .Produces<CreateProductHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
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
