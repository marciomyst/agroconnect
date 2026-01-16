using Agronomia.Api.Infrastructure;
using Agronomia.Crosscutting.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Agronomia.Api.Extensions.Auth;

public static class AuthenticationExtensions
{
    public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<JwtTokenSettings>()
            .Bind(builder.Configuration.GetSection("Jwt"))
            .ValidateFluentValidation()
            .ValidateOnStart();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]
                        ?? throw new InvalidOperationException("JWT Secret not configured")))
            };
        });

        return builder;
    }
}
