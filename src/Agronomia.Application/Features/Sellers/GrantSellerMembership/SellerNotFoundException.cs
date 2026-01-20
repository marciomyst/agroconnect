namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class SellerNotFoundException(Guid sellerId)
    : InvalidOperationException($"Seller '{sellerId}' was not found.");
