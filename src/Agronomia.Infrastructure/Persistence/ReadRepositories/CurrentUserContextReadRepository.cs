using Agronomia.Application.Features.Identity.GetCurrentUserContext;
using Dapper;
using System.Data;
using System.Linq;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

/// <summary>
/// Dapper-based read repository for current user context projections.
/// </summary>
public sealed class CurrentUserContextReadRepository(IDbConnection connection) : ICurrentUserContextReadRepository
{
    private const string UserSql = """
        SELECT
            u."Id"      AS "UserId",
            u."Email"   AS "Email",
            u."Name"    AS "Name"
        FROM users u
        WHERE u."Id" = @UserId
        LIMIT 1;
        """;

    private const string OrganizationsSql = """
        SELECT
            f."Id"      AS "OrganizationId",
            f."Name"    AS "OrganizationName",
            'Farm'      AS "Type",
            fm."Role"   AS "Role"
        FROM farm_memberships fm
        INNER JOIN farms f ON f."Id" = fm."FarmId"
        WHERE fm."UserId" = @UserId

        UNION ALL

        SELECT
            s."Id"              AS "OrganizationId",
            s."CorporateName"   AS "OrganizationName",
            'Seller'            AS "Type",
            sm."Role"           AS "Role"
        FROM seller_memberships sm
        INNER JOIN sellers s ON s."Id" = sm."SellerId"
        WHERE sm."UserId" = @UserId;
        """;

    public async Task<CurrentUserContextResponse?> GetCurrentUserContextAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userCommand = new CommandDefinition(UserSql, new { UserId = userId }, cancellationToken: cancellationToken);
        var user = await connection.QuerySingleOrDefaultAsync<UserRow>(userCommand);
        if (user is null)
        {
            return null;
        }

        var organizationCommand = new CommandDefinition(
            OrganizationsSql,
            new { UserId = userId },
            cancellationToken: cancellationToken);
        var organizations = await connection.QueryAsync<OrganizationRow>(organizationCommand);

        var mappedOrganizations = organizations
            .GroupBy(row => new { row.OrganizationId, row.OrganizationName, row.Type })
            .Select(group => new CurrentUserOrganizationDto(
                group.Key.OrganizationId,
                group.Key.OrganizationName,
                group.Key.Type,
                group.Select(row => row.Role)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray()))
            .ToList();

        return new CurrentUserContextResponse(user.UserId, user.Email, user.Name, mappedOrganizations);
    }

    private sealed class UserRow
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    private sealed class OrganizationRow
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
