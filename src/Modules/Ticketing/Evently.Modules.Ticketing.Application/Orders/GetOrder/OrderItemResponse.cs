namespace Evently.Modules.Ticketing.Application.Orders.GetOrder;

public sealed record OrderItemResponse(
    string OrderItemId,
    string OrderId,
    string TicketTypeId,
    decimal Quantity,
    decimal UnitPrice,
    decimal Price,
    string Currency);
