using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketTypeSoldOutIntegrationEvent : IntegrationEvent
{
    public TicketTypeSoldOutIntegrationEvent(
        string id,
        DateTime occurredOnUtc,
        string ticketTypeId)
        : base(id, occurredOnUtc)
    {
        TicketTypeId = ticketTypeId;
    }

    public string TicketTypeId { get; init; }
}
