using Agronomia.Application.Features.Users;

namespace Agronomia.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Handles retrieval of the currently authenticated user.
/// </summary>
/// <param name="readRepository">Optimized read repository used to load users.</param>
/// <param name="cache">Cache service for user projection.</param>
public sealed class GetCurrentUserQueryHandler(IUserReadRepository readRepository)   
{
    /// <inheritdoc />
    public async Task<UserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await readRepository.GetByIdAsync(request.UserId, cancellationToken);
        return user;
    }
}
