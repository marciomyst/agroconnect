using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Features.Farms.GrantFarmMembership;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Farms.GrantFarmMembership;

public static class GrantFarmMembershipEndpoint
{
    public static void MapGrantFarmMembership(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/farms/{farmId:guid}/memberships", async (
            [FromRoute] Guid farmId,
            [FromBody] GrantFarmMembershipHttpRequest request,
            HttpContext httpContext,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var executorUserIdValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!Guid.TryParse(executorUserIdValue, out var executorUserId))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid token.");
            }

            var command = request.ToCommand(executorUserId, farmId);

            try
            {
                var result = await messageBus.InvokeAsync<GrantFarmMembershipResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/farms/{farmId}/memberships/{response.FarmMembershipId}", response);
            }
            catch (FarmNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Farm not found.");
            }
            catch (UserNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "User not found.");
            }
            catch (FarmMembershipForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "Only a Farm Owner can grant memberships.");
            }
            catch (FarmMembershipAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Membership already exists.");
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
        .WithName("GrantFarmMembership")
        .WithTags("Farms")
        .WithSummary("Grant a farm membership to a user.")
        .WithDescription("Creates a farm membership with the specified role for the target user.")
        .RequireAuthorization("User")
        .Produces<GrantFarmMembershipHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
