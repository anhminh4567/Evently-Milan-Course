using Evently.Modules.Ticketing.Domain.Orders.Enums;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

public sealed record OrderResponse(
    string Id,
    string CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAtUtc);
