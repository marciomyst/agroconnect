using Agronomia.Domain.Aggregates.Users.ValueObjects;

namespace Agronomia.Application.Users;

/// <summary>
/// Lightweight projection of a user used by queries.
/// </summary>
/// <param name="Id">User identifier.</param>
/// <param name="Email">Email address.</param>
/// <param name="Name">Display name.</param>
/// <param name="Type">User type (individual or company member).</param>
/// <param name="Role">Role inside a company context.</param>
/// <param name="CompanyIds">Companies the user belongs to.</param>
/// <param name="CreatedAt">Creation timestamp.</param>
public sealed record UserDto(
    string Id,
    string Email,
    string Name,
    UserRole Role,
    DateTimeOffset CreatedAt);
