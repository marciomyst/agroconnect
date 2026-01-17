using Agronomia.Application;
using Wolverine;
using Wolverine.FluentValidation;

namespace Agronomia.Api.Extensions.Application;

public static class MediatorExtensions
{
    public static WebApplicationBuilder AddMediatorServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(opts =>
        {
            opts.Discovery.IncludeAssembly(typeof(AssemblyReference).Assembly);
            opts.UseFluentValidation();

        });

        return builder;
    }
}
