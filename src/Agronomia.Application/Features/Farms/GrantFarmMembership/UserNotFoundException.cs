namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed class UserNotFoundException(Guid userId)
    : InvalidOperationException($"User '{userId}' was not found.");
