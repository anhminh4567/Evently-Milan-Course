using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Domain.Orders;

public sealed class OrderItem
{
    private OrderItem()
    {
    }

    public string Id { get; private set; }

    public string OrderId { get; private set; }
    // navigation purpose
    public Order? Order { get; set; }
    public string TicketTypeId { get; private set; }
    // navigation purpose
    public TicketType? TicketType { get; set; }  

    public decimal Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    internal static OrderItem Create(string orderId, string ticketTypeId, decimal quantity, decimal unitPrice, string currency)
    {
        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = orderId,
            TicketTypeId = ticketTypeId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Price = quantity * unitPrice,
            Currency = currency
        };

        return orderItem;
    }

}
