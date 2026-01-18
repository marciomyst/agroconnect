using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Agronomia.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework Core repository for <see cref="User"/> aggregates.
/// </summary>
/// <remarks>
/// Creates a new repository bound to the given DbContext.
/// </remarks>
/// <param name="context">Agronomia DbContext instance.</param>
public sealed class UserRepository(AgronomiaDbContext context) : IUserRepository
{
    /// <inheritdoc />
    public IUnitOfWork UnitOfWork => context;

    /// <inheritdoc />
    public void Add(User user)
    {
        context.Users.Add(user);
    }

    /// <inheritdoc />
    public void Update(User user)
    {
        context.Users.Update(user);
    }

    /// <inheritdoc />
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
