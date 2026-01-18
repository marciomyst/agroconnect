using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Application.Features.Authentication;
using Agronomia.Application.Features.Users;
using Agronomia.Infrastructure.Persistence.ReadRepositories;
using Agronomia.Infrastructure.Persistence.Repositories;
using Agronomia.Infrastructure.Security;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class RepositoryExtensions
{
    public static WebApplicationBuilder AddRepositoryServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthenticationReadRepository, AuthenticationReadRepository>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();

        builder.Services.AddSingleton<IPasswordHasher, PasswordHasherAdapter>();

        return builder;
    }
}
