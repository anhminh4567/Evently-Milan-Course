using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed record EventPaymentsRefundedDomainEvent(string eventId) : DomainEvent
{
    public string EventId { get; init; } = eventId;
}
