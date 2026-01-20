using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Diagnostics.Ping;

public sealed record PingCommand : ICommand<string>;
