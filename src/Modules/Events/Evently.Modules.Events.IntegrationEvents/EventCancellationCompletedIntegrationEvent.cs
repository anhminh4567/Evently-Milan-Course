using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCancellationCompletedIntegrationEvent : IntegrationEvent
{
    public EventCancellationCompletedIntegrationEvent(string id, DateTime occurredOnUtc, string eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public string EventId { get; init; }
}
