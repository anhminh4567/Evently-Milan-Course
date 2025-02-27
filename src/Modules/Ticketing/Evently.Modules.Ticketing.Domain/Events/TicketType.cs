using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class TicketType : Entity
{
    private TicketType()
    {
    }

    public string Id { get; private set; }

    public string EventId { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal AvailableQuantity { get; private set; }

    public static TicketType Create(
        string id,
        string eventId,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity,
            AvailableQuantity = quantity
        };

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public Result UpdateQuantity(decimal quantity)
    {
        if (AvailableQuantity < quantity)
        {
            return Result.Failure(TicketTypeErrors.NotEnoughQuantity(AvailableQuantity));
        }

        AvailableQuantity -= quantity;

        if (AvailableQuantity == 0)
        {
            Raise(new TicketTypeSoldOutDomainEvent(Id));
        }

        return Result.Success();
    }
}
