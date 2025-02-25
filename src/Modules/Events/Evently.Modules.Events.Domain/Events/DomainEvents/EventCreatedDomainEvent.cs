using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public record EventCreatedDomainEvent : DomainEvent
{
    public EventCreatedDomainEvent(string eventId) : base()
    {
        EventId = eventId;
    }    
    public string EventId { get; init; }
}
