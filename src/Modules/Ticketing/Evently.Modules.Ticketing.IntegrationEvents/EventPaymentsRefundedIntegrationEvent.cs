using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventPaymentsRefundedIntegrationEvent : IntegrationEvent
{
    public EventPaymentsRefundedIntegrationEvent(string id, DateTime occurredOnUtc, string eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public string EventId { get; init; }
}
