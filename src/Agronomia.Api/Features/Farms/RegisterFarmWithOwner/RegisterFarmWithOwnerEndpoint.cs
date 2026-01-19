using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Features.Farms.RegisterFarmWithOwner;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Farms.RegisterFarmWithOwner;

public static class RegisterFarmWithOwnerEndpoint
{
    public static void MapRegisterFarmWithOwner(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/farms", async (
            [FromBody] RegisterFarmWithOwnerHttpRequest request,
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
                var result = await messageBus.InvokeAsync<RegisterFarmWithOwnerResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/farms/{response.FarmId}", response);
            }
            catch (FarmTaxIdAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Farm with TaxId already exists.");
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
        .WithName("RegisterFarmWithOwner")
        .WithTags("Farms")
        .WithSummary("Register a farm and grant owner membership.")
        .WithDescription("Registers a farm and assigns the authenticated user as the owner.")
        .RequireAuthorization("User")
        .Produces<RegisterFarmWithOwnerHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
