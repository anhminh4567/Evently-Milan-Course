using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed record EventRescheduledDomainEvent(string eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
    : DomainEvent
{
    public string EventId { get; } = eventId;

    public DateTime StartsAtUtc { get; } = startsAtUtc;

    public DateTime? EndsAtUtc { get; } = endsAtUtc;
}
