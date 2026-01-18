using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Diagnostics.Ping;

public sealed class PingCommandHandler : ICommandHandler<PingCommand, string>
{
    public Task<string> HandleAsync(PingCommand command, CancellationToken ct)
        => Task.FromResult("pong");
}
