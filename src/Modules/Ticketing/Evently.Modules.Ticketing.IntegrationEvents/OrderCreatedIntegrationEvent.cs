using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public OrderCreatedIntegrationEvent(
        string id,
        DateTime occuredOnUtc,
        string orderId,
        string customerId,
        decimal totalPrice,
        DateTime createdAtUtc,
        List<OrderItemModel> orderItems)
        : base(id, occuredOnUtc)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalPrice = totalPrice;
        CreatedAtUtc = createdAtUtc;
        OrderItems = orderItems;
    }

    public string OrderId { get; init; }

    public string CustomerId { get; init; }

    public decimal TotalPrice { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public List<OrderItemModel> OrderItems { get; init; }
}
