using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

public sealed record TicketTypeCreatedDomainEvent(string ticketTypeId) : DomainEvent
{
    public string TicketTypeId { get; init; } = ticketTypeId;
}
