using Wolverine;
using Wolverine.FluentValidation;

namespace Agronomia.Api.Extensions.Application;

public static class MediatorExtensions
{
    public static WebApplicationBuilder AddMediatorServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(opts =>
        {
            opts.UseFluentValidation();

        });

        return builder;
    }
}
