using Agronomia.Application.Behaviors;
using Agronomia.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Agronomia.Application.Infrastructure.Wolverine;

public static class WolverineApplicationBuilderExtensions
{
    public static IServiceCollection AddWolverineCqrs(this IServiceCollection services)
    {
        services.AddTransient(typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(TransactionBehavior<,>));
        services.AddScoped<DomainEventsDispatcher>();

        return services;
    }
}
