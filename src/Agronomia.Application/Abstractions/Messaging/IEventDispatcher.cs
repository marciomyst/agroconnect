using Agronomia.Domain.Common;

namespace Agronomia.Application.Abstractions.Messaging;

public interface IEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken ct);
}
