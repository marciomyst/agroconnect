using Agronomia.Domain.Aggregates.Sellers;
using Agronomia.Domain.Aggregates.Users.ValueObjects;
using Agronomia.Domain.SeedWork;

namespace Agronomia.Domain.Aggregates.Users;

/// <summary>
/// Represents a system user, either individual or a company member with a role.
/// </summary>
/// <remarks>
/// <para>
/// Users are typed via <see cref="UserType"/> to distinguish standalone accounts from company members.
/// Company members can be assigned a <see cref="UserRole"/> (e.g., supervisor) and reference
/// one or more tenant memberships via <see cref="Companies"/>.
/// </para>
/// <para>
/// Password is stored as a secure hash; the domain does not enforce hashing but expects the application
/// layer to provide a hashed value.
/// </para>
/// </remarks>
public sealed class User : Entity, IAggregateRoot
{
    private User()
    {
        // Required by EF Core
    }

    /// <summary>
    /// Creates a new user aggregate.
    /// </summary>
    /// <param name="email">User email (unique identifier for authentication).</param>
    /// <param name="password">Secure password hash.</param>
    /// <param name="name">Full name.</param>
    /// <param name="role">User role inside a company context.</param>
    /// <param name="id">Optional identifier. If omitted, a new string Guid is generated.</param>
    /// <param name="createdAt">Optional creation timestamp. Defaults to UTC now.</param>
    public User(
        string email,
        string password,
        string name,
        UserRole role,
        string? id = null,
        DateTimeOffset? createdAt = null)
    {
        Id = id ?? Guid.NewGuid().ToString();
        Email = email;
        Password = password;
        Name = name;
        Role = role;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the user email used for identity.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the secure password hash.
    /// </summary>
    public string Password { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the full name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the role assigned to the user within a company context.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets when the user account was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the sellers this user can manage.
    /// </summary>
    public ICollection<Seller> ManagedSellers { get; private set; } = new List<Seller>();

    /// <summary>
    /// Associates the user with a seller to manage it.
    /// </summary>
    public void AssignSeller(Seller seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        if (ManagedSellers.Any(managedSeller => managedSeller.Id == seller.Id))
        {
            return;
        }

        ManagedSellers.Add(seller);

        if (seller.Managers.Any(manager => manager.Id == Id))
        {
            return;
        }

        seller.Managers.Add(this);
    }
}
