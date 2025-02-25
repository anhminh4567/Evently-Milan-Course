using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public sealed record EventPublishedDomainEvent(string eventId) : DomainEvent
{
    public string EventId { get; init; } = eventId;
}
