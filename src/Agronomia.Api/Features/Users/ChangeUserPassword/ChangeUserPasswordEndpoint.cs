using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Features.Identity.ChangeUserPassword;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Users.ChangeUserPassword;

public static class ChangeUserPasswordEndpoint
{
    public static void MapChangeUserPassword(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/password", async (
            [FromBody] ChangeUserPasswordRequest request,
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
                var result = await messageBus.InvokeAsync<ChangeUserPasswordResult>(command, ct);
                var response = result.FromResult();
                return Results.Ok(response);
            }
            catch (InvalidCurrentPasswordException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Invalid credentials.");
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
        .WithName("ChangeUserPassword")
        .WithTags("Users")
        .WithSummary("Change the current user's password.")
        .WithDescription("Updates the authenticated user's password after validating the current password.")
        .RequireAuthorization("User")
        .Produces<ChangeUserPasswordResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
