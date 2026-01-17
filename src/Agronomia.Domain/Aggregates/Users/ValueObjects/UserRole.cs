namespace Agronomia.Domain.Aggregates.Users.ValueObjects;

/// <summary>
/// Roles available for company members.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Standard company user.
    /// </summary>
    User = 1,

    /// <summary>
    /// Supervisor with elevated permissions.
    /// </summary>
    Supervisor = 2,

    /// <summary>
    /// Full-access administrator role.
    /// </summary>
    Administrator = 3,
}
