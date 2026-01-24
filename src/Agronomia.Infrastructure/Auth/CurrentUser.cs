using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agronomia.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace Agronomia.Infrastructure.Auth;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            var userIdValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(userIdValue, out var userId) ? userId : null;
        }
    }

    public string? Email
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            return httpContext.User.FindFirstValue(ClaimTypes.Email)
                ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Email);
        }
    }
}
