using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;

namespace Agronomia.Application.Abstractions.Identity;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct);

    Task<User?> GetByEmailAsync(Email email, CancellationToken ct);

    Task<User?> GetByIdAsync(Guid userId, CancellationToken ct);

    Task<bool> ExistsAsync(Guid userId, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);
}
