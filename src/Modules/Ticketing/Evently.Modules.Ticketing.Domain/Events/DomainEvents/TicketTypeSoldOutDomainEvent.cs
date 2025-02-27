using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed record TicketTypeSoldOutDomainEvent(string ticketTypeId) : DomainEvent
{
    public string TicketTypeId { get; init; } = ticketTypeId;
}
