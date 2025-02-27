using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

public sealed record TicketArchivedDomainEvent(string ticketId, string code) : DomainEvent
{
    public string TicketId { get; init; } = ticketId;

    public string Code { get; init; } = code;
}
