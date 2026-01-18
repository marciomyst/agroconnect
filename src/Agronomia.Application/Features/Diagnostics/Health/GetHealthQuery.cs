using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Diagnostics.Health;

public sealed record GetHealthQuery : IQuery<string>;
