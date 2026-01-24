using Agronomia.Application.Abstractions.Auth;
using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public sealed class GetCurrentUserContextQueryHandler(
    ICurrentUser currentUser,
    ICurrentUserContextReadRepository readRepository)
    : IQueryHandler<GetCurrentUserContextQuery, CurrentUserContextResponse>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ICurrentUserContextReadRepository _readRepository = readRepository;

    public async Task<CurrentUserContextResponse> HandleAsync(GetCurrentUserContextQuery query, CancellationToken ct)
    {
        if (!_currentUser.UserId.HasValue)
        {
            throw new CurrentUserNotAuthenticatedException();
        }

        var context = await _readRepository.GetCurrentUserContextAsync(_currentUser.UserId.Value, ct);
        if (context is null)
        {
            throw new CurrentUserNotFoundException(_currentUser.UserId.Value);
        }

        return context;
    }
}
