namespace Agronomia.Api.Features.Farms.RegisterFarmWithOwner;

public sealed record RegisterFarmWithOwnerHttpRequest(
    string TaxId,
    string Name
);
