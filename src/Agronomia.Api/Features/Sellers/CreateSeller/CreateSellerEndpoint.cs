using Agronomia.Application.Features.Sellers.CreateSeller;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Agronomia.Api.Features.Sellers.CreateSeller;

/// <summary>
/// Maps the seller creation endpoint.
/// </summary>
public static class CreateSellerEndpoint
{
    /// <summary>
    /// Maps POST /api/sellers to create a new seller.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapCreateSeller(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sellers", async (
            [FromBody] CreateSellerRequest request,
            IMessageBus messageBus,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await messageBus.InvokeAsync<CreateSellerResult>(command, cancellationToken);

            var response = new CreateSellerResponse(result.Id);
            return Results.Created($"/api/sellers/{result.Id}", response);
        })
        .WithName("CreateSeller")
        .WithTags("Sellers")
        .WithSummary("Cria uma nova revenda (seller).")
        .WithDescription("Recebe os dados da revenda e cadastra no sistema.")
        .Produces<CreateSellerResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .AllowAnonymous();
    }
}
