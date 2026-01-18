using Agronomia.Application.Abstractions.Persistence;
using Agronomia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Agronomia.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork(AgronomiaDbContext dbContext) : IUnitOfWork, IAsyncDisposable
{
    private readonly AgronomiaDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("Transaction already started.");
        }

        _transaction = await _dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction.");
        }

        await _dbContext.SaveChangesAsync(ct);
        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
        }
    }
}
