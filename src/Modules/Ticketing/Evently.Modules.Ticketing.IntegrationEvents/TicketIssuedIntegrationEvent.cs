using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketIssuedIntegrationEvent : IntegrationEvent
{
    public TicketIssuedIntegrationEvent(
        string id,
        DateTime occurredOnUtc,
        string ticketId,
        string customerId,
        string eventId,
        string code)
        : base(id, occurredOnUtc)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
        Code = code;
    }

    public string TicketId { get; init; }

    public string CustomerId { get; init; }

    public string EventId { get; init; }

    public string Code { get; init; }
}
