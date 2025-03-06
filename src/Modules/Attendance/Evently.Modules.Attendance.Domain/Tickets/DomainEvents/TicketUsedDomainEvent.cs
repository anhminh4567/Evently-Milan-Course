using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

public sealed record TicketUsedDomainEvent(string ticketId) : DomainEvent
{
    public string TicketId { get; init; } = ticketId;
}
