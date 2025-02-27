using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand(string CustomerId, string TicketTypeId) : ICommand;
