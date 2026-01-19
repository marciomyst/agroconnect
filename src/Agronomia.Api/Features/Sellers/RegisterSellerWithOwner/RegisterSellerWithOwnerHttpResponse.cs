namespace Agronomia.Api.Features.Sellers.RegisterSellerWithOwner;

public sealed record RegisterSellerWithOwnerHttpResponse(
    Guid SellerId,
    Guid SellerMembershipId
);
