using Agronomia.Application.Abstractions.Messaging;
using Agronomia.Domain.Common;

namespace Agronomia.Application.Infrastructure;

/// <summary>
/// Collects domain events from aggregates and dispatches them through the event dispatcher.
/// <para>
/// Expected flow:
/// 1) Aggregates raise events via AddDomainEvent.
/// 2) Application collects events after command execution.
/// 3) Events are dispatched via IEventDispatcher.
/// 4) Aggregates clear their pending events.
/// </para>
/// </summary>
public sealed class DomainEventsDispatcher(IEventDispatcher dispatcher)
{
    private readonly IEventDispatcher _dispatcher = dispatcher;

    public async Task DispatchAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken ct)
    {
        var events = aggregates
            .SelectMany(aggregate => aggregate.DomainEvents)
            .ToList();

        if (events.Count == 0)
        {
            return;
        }

        await _dispatcher.DispatchAsync(events, ct);

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }
    }
}
