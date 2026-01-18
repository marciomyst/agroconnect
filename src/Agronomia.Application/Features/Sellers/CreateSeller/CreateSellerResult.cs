namespace Agronomia.Application.Features.Sellers.CreateSeller;

/// <summary>
/// Result of seller creation.
/// </summary>
/// <param name="Id">Identifier of the created seller.</param>
public sealed record CreateSellerResult(string Id);
