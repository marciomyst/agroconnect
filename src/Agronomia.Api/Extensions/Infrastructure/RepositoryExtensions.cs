using Agronomia.Application.Abstractions.Auth;
using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.Identity;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Application.Abstractions.Organizations;
using Agronomia.Application.Abstractions.Security;
using Agronomia.Application.Features.Identity.GetCurrentUserContext;
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
        builder.Services.AddScoped<ICurrentUserContextReadRepository, CurrentUserContextReadRepository>();

        builder.Services.AddScoped<IProductRepository, EfProductRepository>();
        builder.Services.AddScoped<ISellerProductRepository, EfSellerProductRepository>();
        builder.Services.AddScoped<IPurchaseIntentRepository, EfPurchaseIntentRepository>();

        builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();
        builder.Services.AddScoped<ISellerCatalogReadRepository, SellerCatalogReadRepository>();
        builder.Services.AddScoped<IMarketplaceReadRepository, MarketplaceReadRepository>();
        builder.Services.AddScoped<IPurchaseIntentReadRepository, PurchaseIntentReadRepository>();

        builder.Services.AddScoped<ISellerRepository, EfSellerRepository>();
        builder.Services.AddScoped<ISellerMembershipRepository, EfSellerMembershipRepository>();
        builder.Services.AddScoped<IFarmRepository, EfFarmRepository>();
        builder.Services.AddScoped<IFarmMembershipRepository, EfFarmMembershipRepository>();

        builder.Services.AddSingleton<IPasswordHasher, PasswordHasherAdapter>();
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return builder;
    }
}
