namespace Agronomia.Domain.SeedWork;

/// <summary>
/// Marker interface to identify aggregate root entities within the domain model.
/// <para>
/// In Domain-Driven Design (DDD), an aggregate root is the main entity that controls access to the aggregate,
/// ensuring consistency and encapsulation of business rules. Only aggregate roots should be accessed directly
/// by repositories, while child entities and value objects are managed through the root.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// This interface is intentionally left empty and serves only as a marker to enforce DDD constraints at compile time.
/// By constraining repositories and certain operations to types implementing <see cref="IAggregateRoot"/>, the domain model
/// maintains clear aggregate boundaries and prevents accidental persistence or retrieval of non-root entities.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// public class Order : Entity, IAggregateRoot
/// {
///     // Order-specific properties and methods
/// }
/// </code>
/// </para>
/// </remarks>
public interface IAggregateRoot
{
}
