using Agronomia.Application.Abstractions.Auth;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Application.Features.Users;
using Agronomia.Infrastructure.Auth;
using Agronomia.Infrastructure.Persistence.Repositories;
using Agronomia.Infrastructure.Persistence.ReadRepositories;
using Agronomia.Infrastructure.Security;

namespace Agronomia.Api.Extensions.Infrastructure;

public static class RepositoryExtensions
{
    public static WebApplicationBuilder AddRepositoryServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();

        builder.Services.AddScoped<ISellerRepository, EfSellerRepository>();
        builder.Services.AddScoped<ISellerMembershipRepository, EfSellerMembershipRepository>();

        builder.Services.AddSingleton<IPasswordHasher, PasswordHasherAdapter>();
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return builder;
    }
}
