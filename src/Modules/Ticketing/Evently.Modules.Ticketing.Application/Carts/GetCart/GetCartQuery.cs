using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Carts.GetCart;

public sealed record GetCartQuery(string CustomerId) : IQuery<Cart>;
