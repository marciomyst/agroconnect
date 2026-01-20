using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.RegisterSellerWithOwner;

public static class RegisterSellerWithOwnerEndpoint
{
    public static void MapRegisterSellerWithOwner(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sellers", async (
            [FromBody] RegisterSellerWithOwnerHttpRequest request,
            HttpContext httpContext,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var userIdValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid token.");
            }

            var command = request.ToCommand(userId);

            try
            {
                var result = await messageBus.InvokeAsync<RegisterSellerWithOwnerResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/sellers/{response.SellerId}", response);
            }
            catch (SellerTaxIdAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Seller with TaxId already exists.");
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }
        })
        .WithName("RegisterSellerWithOwner")
        .WithTags("Sellers")
        .WithSummary("Register a seller and grant owner membership.")
        .WithDescription("Registers a seller and assigns the authenticated user as the owner.")
        .RequireAuthorization("User")
        .Produces<RegisterSellerWithOwnerHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
