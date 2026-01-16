using Agronomia.Application.Users;
using Agronomia.Domain.Interfaces;
using Agronomia.Infrastructure.Persistence.ReadRepositories;
using Agronomia.Infrastructure.Persistence.Repositories;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class RepositoryExtensions
{
    public static WebApplicationBuilder AddRepositoryServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();

        return builder;
    }
}
