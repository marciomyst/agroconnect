using Agronomia.Application;
using Wolverine;

namespace Agronomia.Application.Infrastructure.Wolverine;

public static class WolverineConventions
{
    public static void ApplyCqrsConventions(this WolverineOptions options)
    {
        options.Discovery.IncludeAssembly(typeof(AssemblyReference).Assembly);
        // Behaviors can be wired into the Wolverine pipeline here later.
    }
}
