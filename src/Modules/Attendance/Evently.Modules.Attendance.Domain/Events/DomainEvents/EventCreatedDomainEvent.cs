using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Events.DomainEvents;

public sealed record EventCreatedDomainEvent(
    string eventId,
    string title,
    string description,
    string location,
    DateTime startsAtUtc,
    DateTime? endsAtUtc) : DomainEvent
{
    public string EventId { get; init; } = eventId;

    public string Title { get; init; } = title;

    public string Description { get; init; } = description;

    public string Location { get; init; } = location;

    public DateTime StartsAtUtc { get; init; } = startsAtUtc;

    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}
