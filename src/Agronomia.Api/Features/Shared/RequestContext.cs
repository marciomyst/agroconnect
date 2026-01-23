using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Agronomia.Api.Features.Shared;

internal static class RequestContext
{
    private const string OrganizationHeader = "X-Organization-Id";

    public static bool TryGetUserId(HttpContext httpContext, out Guid userId)
    {
        var userIdValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userIdValue, out userId);
    }

    public static bool TryGetOrganizationId(HttpContext httpContext, out Guid organizationId)
    {
        if (!httpContext.Request.Headers.TryGetValue(OrganizationHeader, out var header))
        {
            organizationId = Guid.Empty;
            return false;
        }

        return Guid.TryParse(header.ToString(), out organizationId);
    }
}
