namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class SellerMembershipAlreadyExistsException(Guid sellerId, Guid userId, string role)
    : InvalidOperationException($"Membership already exists for Seller '{sellerId}', User '{userId}' with Role '{role}'.");
