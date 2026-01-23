using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.SellerCatalog;
using Agronomia.Application.Features.SellerCatalog.CreateSellerProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.Catalog.CreateSellerProduct;

public static class CreateSellerProductEndpoint
{
    public static void MapCreateSellerProduct(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sellers/{sellerId:guid}/catalog", async (
            [FromRoute] Guid sellerId,
            [FromBody] CreateSellerProductHttpRequest request,
            HttpContext httpContext,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            if (!RequestContext.TryGetUserId(httpContext, out var userId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid token.");
            }

            if (!RequestContext.TryGetOrganizationId(httpContext, out var organizationId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request",
                    detail: "Active organization is required.");
            }

            if (organizationId != sellerId)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "Seller context does not match the active organization.");
            }

            var command = request.ToCommand(sellerId, userId);

            try
            {
                var result = await messageBus.InvokeAsync<CreateSellerProductResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/sellers/{sellerId}/catalog/{response.SellerProductId}", response);
            }
            catch (SellerNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Seller not found.");
            }
            catch (ProductNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Product not found.");
            }
            catch (SellerCatalogForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot manage this seller catalog.");
            }
            catch (SellerProductAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Seller already offers this product.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("CreateSellerProduct")
        .WithTags("SellerCatalog")
        .WithSummary("Add product to seller catalog.")
        .WithDescription("Creates a seller-specific product offer.")
        .RequireAuthorization("User")
        .Produces<CreateSellerProductHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
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
