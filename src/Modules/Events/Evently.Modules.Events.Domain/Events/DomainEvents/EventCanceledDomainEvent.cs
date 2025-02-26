using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public sealed record EventCanceledDomainEvent(string eventId) : DomainEvent
{
    public string EventId { get; init; } = eventId;
}
