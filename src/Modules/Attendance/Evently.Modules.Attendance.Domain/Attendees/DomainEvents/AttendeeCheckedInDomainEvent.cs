using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

public sealed record AttendeeCheckedInDomainEvent(string attendeeId, string eventId) : DomainEvent
{
    public string AttendeeId { get; init; } = attendeeId;

    public string EventId { get; init; } = eventId;
}
