namespace Agronomia.Api.Features.Farms.RegisterFarmWithOwner;

public sealed record RegisterFarmWithOwnerHttpResponse(
    Guid FarmId,
    Guid FarmMembershipId
);
