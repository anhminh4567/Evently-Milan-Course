using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventTicketsArchivedIntegrationEvent : IntegrationEvent
{
    public EventTicketsArchivedIntegrationEvent(string id, DateTime occurredOnUtc, string eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public string EventId { get; init; }
}
