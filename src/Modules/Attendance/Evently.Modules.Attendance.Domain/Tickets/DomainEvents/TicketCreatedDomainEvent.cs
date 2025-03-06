using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

public sealed record TicketCreatedDomainEvent(string ticketId, string eventId) : DomainEvent
{
    public string TicketId { get; init; } = ticketId;

    public string EventId { get; init; } = eventId;
}
