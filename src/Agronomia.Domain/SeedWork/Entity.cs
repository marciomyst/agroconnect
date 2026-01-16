namespace Agronomia.Domain.SeedWork;

/// <summary>
/// Serves as the base class for all domain entities, providing a unique identity and management of domain events.
/// <para>
/// In Domain-Driven Design (DDD), entities are objects that are distinguished by their unique identifier rather than their attributes.
/// This class also supports the registration and management of domain events, enabling side effects and business rules to be handled
/// in a decoupled manner.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Entity"/> base class ensures that all entities have a unique identifier and provides infrastructure for
/// domain event management. Domain events are collected during the lifetime of the entity and are typically dispatched
/// by the <c>DbContext</c> (or Unit of Work) before changes are persisted.
/// </para>
/// <para>
/// Equality is based on the unique identifier and type, ensuring that two entities with the same ID and type are considered equal.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// public class Order : Entity
/// {
///     // Order-specific properties and methods
/// }
/// </code>
/// </para>
/// </remarks>
public abstract class Entity
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Guid"/> as the standard identifier type for entities in a microservices environment.
    /// The setter is protected to prevent external modification.
    /// </remarks>
    public virtual string Id { get; protected set; } = string.Empty;

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns>
    /// <c>true</c> if the specified object is an entity of the same type and has the same ID; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var item = (Entity) obj;

        // If the ID is default, the entity has not been persisted and cannot be considered equal.
        if (string.IsNullOrWhiteSpace(item.Id) || string.IsNullOrWhiteSpace(Id))
        {
            return false;
        }

        // Consider proxy types equivalent to their unproxied counterparts to avoid false negatives.
        return item.Id == Id && AreEntityTypesCompatible(GetUnproxiedType(), item.GetUnproxiedType());
    }

    /// <summary>
    /// Returns a hash code for the entity, based on its type and unique identifier.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetUnproxiedType(), Id);
    }

    /// <summary>
    /// Determines whether two entities are equal.
    /// </summary>
    /// <param name="left">The first entity to compare.</param>
    /// <param name="right">The second entity to compare.</param>
    /// <returns>
    /// <c>true</c> if both entities are <c>null</c> or are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two entities are not equal.
    /// </summary>
    /// <param name="left">The first entity to compare.</param>
    /// <param name="right">The second entity to compare.</param>
    /// <returns>
    /// <c>true</c> if the entities are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    private static bool AreEntityTypesCompatible(Type first, Type second)
    {
        return first == second || first.IsAssignableFrom(second) || second.IsAssignableFrom(first);
    }

    private Type GetUnproxiedType()
    {
        Type type = GetType();

        // EF Core generates proxies that derive from the entity type; treat them as the base type for equality/hash.
        if ((type.Namespace == "Castle.Proxies" || type.Name.EndsWith("Proxy", StringComparison.OrdinalIgnoreCase))
            && type.BaseType is { } baseType)
        {
            return baseType;
        }

        return type;
    }
}
