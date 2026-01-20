using Agronomia.Application.Features.Identity.RegisterUser;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Users.RegisterUser;

public static class RegisterUserEndpoint
{
    public static void MapRegisterUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/register", async (
            [FromBody] RegisterUserRequest request,
            IMessageBus messageBus,
            CancellationToken ct) =>
        {
            var command = request.ToCommand();

            try
            {
                var result = await messageBus.InvokeAsync<RegisterUserResult>(command, ct);
                var response = result.FromResult();
                return Results.Created($"/api/users/{response.UserId}", response);
            }
            catch (UserEmailAlreadyExistsException)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "Email already exists.");
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
        .WithName("RegisterUser")
        .WithTags("Users")
        .WithSummary("Register a user.")
        .WithDescription("Registers a user with name, email, and password.")
        .AllowAnonymous()
        .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
