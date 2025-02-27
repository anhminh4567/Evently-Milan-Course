using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed record EventCanceledDomainEvent(string eventId) : DomainEvent
{
    public string EventId { get; } = eventId;
}
