namespace Agronomia.Domain.Common;

/// <summary>
/// Defines the contract for the Unit of Work pattern, coordinating the writing out of changes and the resolution of concurrency problems.
/// <para>
/// In Domain-Driven Design (DDD), the Unit of Work pattern ensures that all changes to aggregates are tracked and persisted as a single business transaction,
/// maintaining consistency and integrity across the domain model.
/// </para>
/// <para>
/// Typically, the <c>DbContext</c> in Entity Framework Core is the concrete implementation of this interface.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// The Unit of Work pattern allows multiple operations to be performed on the domain model, with all changes being committed or rolled back together.
/// This interface should be implemented in the infrastructure layer and injected into application services or command handlers.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// // Application service or command handler
/// movieRepository.Add(movie);
/// // ... other operations ...
/// await unitOfWork.SaveEntitiesAsync(cancellationToken);
/// </code>
/// </para>
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves all changes made in the current context to the database as a single transaction,
    /// and dispatches any domain events raised by aggregates.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// <c>true</c> if the operation succeeds; otherwise, an exception is thrown.
    /// </returns>
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
