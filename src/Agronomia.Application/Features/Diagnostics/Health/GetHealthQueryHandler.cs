using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Diagnostics.Health;

public sealed class GetHealthQueryHandler : IQueryHandler<GetHealthQuery, string>
{
    public Task<string> HandleAsync(GetHealthQuery query, CancellationToken ct)
        => Task.FromResult("ok");
}
