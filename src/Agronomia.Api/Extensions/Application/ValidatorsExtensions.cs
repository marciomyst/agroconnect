using Agronomia.Application;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ApplicationAssembly = Agronomia.Application.AssemblyReference;
using CrosscuttingAssembly = Agronomia.Crosscutting.AssemblyReference;
using DomainAssembly = Agronomia.Domain.AssemblyReference;

namespace Agronomia.Api.Extensions.Application;

public static class ValidatorsExtensions
{
    public static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(
            typeof(ApplicationAssembly).Assembly,
            includeInternalTypes: true);

        builder.Services.AddValidatorsFromAssembly(
            typeof(DomainAssembly).Assembly,
            includeInternalTypes: true);

        // Settings validators must be singleton to support options validation at startup
        builder.Services.AddValidatorsFromAssembly(
            typeof(CrosscuttingAssembly).Assembly,
            ServiceLifetime.Singleton,
            includeInternalTypes: true);

        return builder;
    }
}
