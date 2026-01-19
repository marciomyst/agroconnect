using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Features.Sellers.GrantSellerMembership;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.GrantSellerMembership;

public static class GrantSellerMembershipEndpoint
{
    public static void MapGrantSellerMembership(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sellers/{sellerId:guid}/memberships", async (
            [FromRoute] Guid sellerId,
            [FromBody] GrantSellerMembershipHttpRequest request,
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

            var command = request.ToCommand(executorUserId, sellerId);

            try
            {
                var result = await messageBus.InvokeAsync<GrantSellerMembershipResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/sellers/{sellerId}/memberships/{response.SellerMembershipId}", response);
            }
            catch (SellerNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "Seller not found.");
            }
            catch (UserNotFoundException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "User not found.");
            }
            catch (SellerMembershipForbiddenException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "Only a Seller Owner can grant memberships.");
            }
            catch (SellerMembershipAlreadyExistsException)
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
        .WithName("GrantSellerMembership")
        .WithTags("Sellers")
        .WithSummary("Grant a seller membership to a user.")
        .WithDescription("Creates a seller membership with the specified role for the target user.")
        .RequireAuthorization("User")
        .Produces<GrantSellerMembershipHttpResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
