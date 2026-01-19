using Agronomia.Application.Features.Identity.AuthenticateUser;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Auth.Authenticate;

public static class AuthenticateUserEndpoint
{
    public static void MapAuthenticateUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/authenticate", async (
            [FromBody] AuthenticateUserRequest request,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var command = request.ToCommand();

            try
            {
                var result = await messageBus.InvokeAsync<AuthenticateUserResult>(command, ct);
                var response = result.FromResult();
                return Results.Ok(response);
            }
            catch (InvalidCredentialsException)
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
        .WithName("AuthenticateUser")
        .WithTags("Auth")
        .WithSummary("Authenticate user and return a JWT access token.")
        .WithDescription("Authenticates a user by email and password, returning a JWT access token.")
        .AllowAnonymous()
        .Produces<AuthenticateUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
