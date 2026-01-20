namespace Agronomia.Api.Features.Sellers.RegisterSellerWithOwner;

public sealed record RegisterSellerWithOwnerHttpRequest(
    string TaxId,
    string CorporateName
);
