namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class UserNotFoundException(Guid userId)
    : InvalidOperationException($"User '{userId}' was not found.");
