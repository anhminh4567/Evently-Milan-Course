using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class TicketTypePriceChangedIntegrationEvent : IntegrationEvent
{
    public TicketTypePriceChangedIntegrationEvent(string id, DateTime occurredOnUtc, string ticketTypeId, decimal price)
        : base(id, occurredOnUtc)
    {
        TicketTypeId = ticketTypeId;
        Price = price;
    }

    public string TicketTypeId { get; init; }

    public decimal Price { get; init; }
}
