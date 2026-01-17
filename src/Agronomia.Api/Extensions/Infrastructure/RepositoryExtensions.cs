using Agronomia.Application.Features.Authentication;
using Agronomia.Application.Features.Users;
using Agronomia.Domain.Aggregates.Users;
using Agronomia.Infrastructure.Persistence.ReadRepositories;
using Agronomia.Infrastructure.Persistence.Repositories;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class RepositoryExtensions
{
    public static WebApplicationBuilder AddRepositoryServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthenticationReadRepository, AuthenticationReadRepository>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();

        return builder;
    }
}
