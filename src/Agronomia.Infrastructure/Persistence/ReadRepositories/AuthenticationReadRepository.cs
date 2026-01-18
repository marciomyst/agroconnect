using Agronomia.Application.Features.Authentication;
using Agronomia.Domain.Aggregates.Users.ValueObjects;
using Dapper;
using System.Data;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

/// <summary>
/// Dapper-based read repository for user projections.
/// </summary>
/// <param name="connection">Scoped database connection for read operations.</param>
public sealed class AuthenticationReadRepository(IDbConnection connection) : IAuthenticationReadRepository
{
    private sealed class AuthenticationDbRow
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    private const string GetByIdSql = """
        SELECT
            u."Id"              AS "Id",
            u."Email"           AS "Email",
            u."Password"        AS "Password",
            u."Name"            AS "Name",
            u."Role"::text      AS "Role",
            u."CreatedAt"       AS "CreatedAt"
        FROM users u
        WHERE u."Id" = @UserId;
        """;

    private const string GetByEmailSql = """
        SELECT
            u."Id"              AS "Id",
            u."Email"           AS "Email",
            u."Password"        AS "Password",
            u."Name"            AS "Name",
            u."Role"::text      AS "Role",
            u."CreatedAt"       AS "CreatedAt"
        FROM users u
        WHERE u."Email" = @Email;
        """;

    public async Task<AuthenticationUserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            GetByEmailSql,
            new { Email = email },
            cancellationToken: cancellationToken);

        var row = await connection.QuerySingleOrDefaultAsync<AuthenticationDbRow>(command);
        return Map(row);
    }

    /// <inheritdoc />
    public async Task<AuthenticationUserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            GetByIdSql,
            new { UserId = userId },
            cancellationToken: cancellationToken);

        var row = await connection.QuerySingleOrDefaultAsync<AuthenticationDbRow>(command);
        return Map(row);
    }

    private static AuthenticationUserDto? Map(AuthenticationDbRow? row)
    {
        if (row is null)
        {
            return null;
        }

        if (!TryParseRole(row.Role, out var role))
        {
            return null;
        }

        var createdAt = row.CreatedAt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(row.CreatedAt, DateTimeKind.Utc)
            : row.CreatedAt;

        return new AuthenticationUserDto(
            row.Id,
            row.Email,
            row.Password,
            row.Name,
            role,
            new DateTimeOffset(createdAt));
    }

    private static bool TryParseRole(string value, out UserRole role)
    {
        if (int.TryParse(value, out var roleInt) && Enum.IsDefined(typeof(UserRole), roleInt))
        {
            role = (UserRole)roleInt;
            return true;
        }

        if (Enum.TryParse<UserRole>(value, ignoreCase: true, out var parsed))
        {
            role = parsed;
            return true;
        }

        role = default;
        return false;
    }
}
