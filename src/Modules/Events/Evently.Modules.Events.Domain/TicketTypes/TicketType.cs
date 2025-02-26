using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

namespace Evently.Modules.Events.Domain.TicketTypes;

public sealed class TicketType : Entity
{
    private TicketType()
    {
    }

    public string Id { get; private set; }

    public string EventId { get; private set; }
    public Event? Event { get; set; }
    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    public decimal Quantity { get; private set; }

    public static TicketType Create(
        Event @event,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = Guid.NewGuid().ToString(),
            EventId = @event.Id,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity
        };
        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        if (Price == price)
        {
            return;
        }
        Price = price;
        Raise(new TicketTypePriceChangedDomainEvent(Id, Price));
    }
}
