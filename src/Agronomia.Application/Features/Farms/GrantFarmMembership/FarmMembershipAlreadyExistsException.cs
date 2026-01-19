namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class FarmMembershipAlreadyExistsException(Guid farmId, Guid userId, string role)
    : InvalidOperationException($"Membership already exists for Farm '{farmId}', User '{userId}' with Role '{role}'.");
