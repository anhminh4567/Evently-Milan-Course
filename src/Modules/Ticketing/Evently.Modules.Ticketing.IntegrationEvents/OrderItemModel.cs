namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class OrderItemModel
{
    public string Id { get; init; }

    public string OrderId { get; init; }

    public string TicketTypeId { get; init; }

    public decimal Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal Price { get; init; }

    public string Currency { get; init; }
}
