namespace Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

public sealed record RegisterSellerWithOwnerResult(
    Guid SellerId,
    Guid SellerMembershipId
);
