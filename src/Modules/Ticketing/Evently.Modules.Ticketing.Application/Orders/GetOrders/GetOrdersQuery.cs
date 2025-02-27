using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

public sealed record GetOrdersQuery(string CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;
