using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

public sealed record DuplicateCheckInAttemptedDomainEvent(string attendeeId, string eventId, string ticketId, string ticketCode)
    : DomainEvent
{
    public string AttendeeId { get; init; } = attendeeId;

    public string EventId { get; init; } = eventId;

    public string TicketId { get; init; } = ticketId;

    public string TicketCode { get; init; } = ticketCode;
}
