namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class FarmNotFoundException(Guid farmId)
    : InvalidOperationException($"Farm '{farmId}' was not found.");
