namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public interface ICurrentUserContextReadRepository
{
    Task<CurrentUserContextResponse?> GetCurrentUserContextAsync(Guid userId, CancellationToken cancellationToken = default);
}
