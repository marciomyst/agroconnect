using Agronomia.Application;
using FluentValidation;

namespace Agronomia.Api.Extensions.Application;

public static class ValidatorsExtensions
{
    public static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly, includeInternalTypes: true);

        return builder;
    }
}
