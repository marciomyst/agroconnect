using Agronomia.Application.Abstractions.Identity;
using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;
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
    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct)
    {
        return context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email.Value == email.Value, ct);
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken ct)
    {
        return context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Email == email, ct);
    }

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct)
    {
        return context.Users
            .SingleOrDefaultAsync(user => user.Id == userId, ct);
    }

    public Task<bool> ExistsAsync(Guid userId, CancellationToken ct)
    {
        return context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == userId, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await context.Users.AddAsync(user, ct);
    }
}
