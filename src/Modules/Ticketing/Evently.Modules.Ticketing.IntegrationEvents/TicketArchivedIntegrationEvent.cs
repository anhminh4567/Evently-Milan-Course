using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketArchivedIntegrationEvent : IntegrationEvent
{
    public TicketArchivedIntegrationEvent(
        string id,
        DateTime occurredOnUtc,
        string ticketId,
        string code)
        : base(id, occurredOnUtc)
    {
        TicketId = ticketId;
        Code = code;
    }

    public string TicketId { get; init; }

    public string Code { get; init; }
}
