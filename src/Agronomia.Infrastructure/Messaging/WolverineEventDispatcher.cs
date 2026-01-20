using Agronomia.Application.Abstractions.Messaging;
using Agronomia.Domain.Common;
using Wolverine;

namespace Agronomia.Infrastructure.Messaging;

public sealed class WolverineEventDispatcher(IMessageBus messageBus) : IEventDispatcher
{
    private readonly IMessageBus _messageBus = messageBus;

    public async Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken ct)
    {
        foreach (var domainEvent in domainEvents)
        {
            ct.ThrowIfCancellationRequested();
            await _messageBus.PublishAsync(domainEvent);
        }
    }
}
