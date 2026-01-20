using Agronomia.Application.Features.Users;
using Agronomia.Domain.Aggregates.Users.ValueObjects;
using Dapper;
using System.Data;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

/// <summary>
/// Dapper-based read repository for user projections.
/// </summary>
/// <param name="connection">Scoped database connection for read operations.</param>
public sealed class UserReadRepository(IDbConnection connection) : IUserReadRepository
{
    private const string GetByIdSql = """
        SELECT
            u."Id"              AS "Id",
            u."Email"           AS "Email",
            u."Name"            AS "Name",
            u."Role"            AS "Role",
            u."CreatedAt"       AS "CreatedAt"
        FROM users u
        WHERE u."Id" = @UserId
        GROUP BY u."Id"
        LIMIT 1;
        """;

    /// <inheritdoc />
    public async Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            GetByIdSql,
            new { UserId = userId },
            cancellationToken: cancellationToken);

        var row = await connection.QuerySingleOrDefaultAsync<UserRow>(command);
        if (row is null)
        {
            return null;
        }

        var createdAt = new DateTimeOffset(DateTime.SpecifyKind(row.CreatedAt, DateTimeKind.Utc));

        return new UserDto(
            row.Id,
            row.Email,
            row.Name,
            Enum.Parse<UserRole>(row.Role, ignoreCase: true),
            createdAt);
    }

    private sealed class UserRow
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
