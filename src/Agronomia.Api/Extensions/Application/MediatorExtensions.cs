using Agronomia.Application;
using Agronomia.Application.Infrastructure.Wolverine;
using Wolverine;
using Wolverine.FluentValidation;

namespace Agronomia.Api.Extensions.Application;

public static class MediatorExtensions
{
    public static WebApplicationBuilder AddMediatorServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddWolverineCqrs();

        builder.Host.UseWolverine(opts =>
        {
            opts.ApplyCqrsConventions();
            opts.UseFluentValidation();

        });

        return builder;
    }
}
