using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Orders.DomainEvents;

public sealed record OrderCreatedDomainEvent(string orderId) : DomainEvent
{
    public string OrderId { get; init; } = orderId;
}
