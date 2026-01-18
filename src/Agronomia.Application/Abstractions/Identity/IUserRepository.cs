using Agronomia.Domain.Identity;
using Agronomia.Domain.Identity.ValueObjects;

namespace Agronomia.Application.Abstractions.Identity;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);
}
