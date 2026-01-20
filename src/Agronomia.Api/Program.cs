
using Agronomia.Api.Extensions;

namespace Agronomia.Api;

/// <summary>
/// Entry point for the Agronomia API application.
/// </summary>
/// <remarks>
/// Bootstraps the web host, registers dependencies via <see cref="ProgramExtensions.AddDependencies(WebApplicationBuilder)"/>,
/// builds the app, wires middlewares/endpoints, and starts the HTTP server.
/// </remarks>
public class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddDependencies();

        WebApplication app = builder.Build();
        app.AddMiddlewares();

        app.Run();
    }
}