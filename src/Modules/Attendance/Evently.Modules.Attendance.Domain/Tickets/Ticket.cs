using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Tickets;

public sealed class Ticket : Entity
{
    private Ticket()
    {
    }

    public string Id { get; private set; }

    public string AttendeeId { get; private set; }

    public string EventId { get; private set; }

    public string Code { get; private set; }

    public DateTime? UsedAtUtc { get; private set; }

    public static Ticket Create(string ticketId, Attendee attendee, Event @event, string code)
    {
        var ticket = new Ticket
        {
            Id = ticketId,
            AttendeeId = attendee.Id,
            EventId = @event.Id,
            Code = code
        };

        ticket.Raise(new TicketCreatedDomainEvent(ticket.Id, ticket.EventId));

        return ticket;
    }

    internal void MarkAsUsed()
    {
        UsedAtUtc = DateTime.UtcNow;

        Raise(new TicketUsedDomainEvent(Id));
    }
}
