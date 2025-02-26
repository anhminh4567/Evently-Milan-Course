using Evently.Common.Domain;
namespace Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

public sealed record TicketTypePriceChangedDomainEvent(string ticketTypeId, decimal price) : DomainEvent
{
    public string TicketTypeId { get; init; } = ticketTypeId;

    public decimal Price { get; init; } = price;
}
