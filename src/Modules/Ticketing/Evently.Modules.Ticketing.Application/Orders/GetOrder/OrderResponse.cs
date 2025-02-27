using Evently.Modules.Ticketing.Domain.Orders.Enums;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrder;

public sealed record OrderResponse(
    string Id,
    string CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAtUtc)
{
    public List<OrderItemResponse> OrderItems { get; } = [];
}
