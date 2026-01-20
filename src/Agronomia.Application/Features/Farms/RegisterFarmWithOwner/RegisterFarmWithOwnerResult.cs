namespace Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

public sealed record RegisterFarmWithOwnerResult(
    Guid FarmId,
    Guid FarmMembershipId
);
