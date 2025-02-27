using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

public sealed record TicketCreatedDomainEvent(string ticketId) : DomainEvent
{
    public string TicketId { get; init; } = ticketId;
}
