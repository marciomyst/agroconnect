using Agronomia.Application.Users;

namespace Agronomia.Application.Users.GetCurrentUser;

/// <summary>
/// Query that returns the authenticated user's details.
/// </summary>
/// <param name="UserId">Identifier from the authenticated principal.</param>
public sealed record GetCurrentUserQuery(string UserId);
