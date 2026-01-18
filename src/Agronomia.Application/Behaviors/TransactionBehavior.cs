using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Persistence;

namespace Agronomia.Application.Behaviors;

/// <summary>
/// Wraps command execution in a transactional boundary.
/// </summary>
public sealed class TransactionBehavior<TCommand, TResult>(IUnitOfWork unitOfWork)
    where TCommand : ICommand<TResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResult> HandleAsync(
        TCommand command,
        Func<CancellationToken, Task<TResult>> next,
        CancellationToken ct)
    {
        // Future flow:
        // 1) _unitOfWork.BeginTransactionAsync(ct)
        // 2) Execute handler
        // 3) Dispatch domain events via DomainEventsDispatcher and IEventDispatcher
        // 4) _unitOfWork.CommitAsync(ct)
        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var result = await next(ct);
            await _unitOfWork.CommitAsync(ct);
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
