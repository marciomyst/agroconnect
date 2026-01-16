using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Agronomia.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request, ICommandBus bus, CancellationToken cancellationToken) =>
        {
            var response = await bus.InvokeAsync<LoginResponse>(request, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("AuthLogin");
    }

    public static LoginResponse Handle(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EmailOrPhone) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse(false, null, "Email or password is required.");
        }

        // TODO: replace with real authentication flow and token issuance.
        return new LoginResponse(true, "sample-token", "Login successful.");
    }
}
