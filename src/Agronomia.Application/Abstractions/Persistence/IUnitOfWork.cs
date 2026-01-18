namespace Agronomia.Application.Abstractions.Persistence;

/// <summary>
/// Abstracts transactional boundaries for command execution.
/// </summary>
public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken ct);

    Task CommitAsync(CancellationToken ct);

    Task RollbackAsync(CancellationToken ct);
}
