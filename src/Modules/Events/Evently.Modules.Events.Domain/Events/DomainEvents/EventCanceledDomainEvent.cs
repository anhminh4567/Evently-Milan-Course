using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public sealed record EventCanceledDomainEvent(string eventId) : DomainEvent
{
    public string EventId { get; init; } = eventId;
}
