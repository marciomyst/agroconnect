using Agronomia.Api.Features.Shared;
using Agronomia.Application.Features.SellerCatalog;
using Agronomia.Application.Features.SellerCatalog.UpdateSellerProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.Catalog.UpdateSellerProduct;

public static class UpdateSellerProductEndpoint
{
    public static void MapUpdateSellerProduct(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/sellers/{sellerId:guid}/catalog/{sellerProductId:guid}", async (
            [FromRoute] Guid sellerId,
            [FromRoute] Guid sellerProductId,
            [FromBody] UpdateSellerProductHttpRequest request,
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

            var command = request.ToCommand(sellerId, sellerProductId, userId);

            try
            {
                var result = await messageBus.InvokeAsync<UpdateSellerProductResult>(command, ct);
                var response = result.FromResult();
                return Results.Ok(response);
            }
            catch (SellerNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Seller not found.");
            }
            catch (SellerProductNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Seller product not found.");
            }
            catch (SellerCatalogForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "User cannot manage this seller catalog.");
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ToValidationErrors(ex));
            }
        })
        .WithName("UpdateSellerProduct")
        .WithTags("SellerCatalog")
        .WithSummary("Update seller catalog item.")
        .WithDescription("Updates price and availability for a seller product.")
        .RequireAuthorization("User")
        .Produces<UpdateSellerProductHttpResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);
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
