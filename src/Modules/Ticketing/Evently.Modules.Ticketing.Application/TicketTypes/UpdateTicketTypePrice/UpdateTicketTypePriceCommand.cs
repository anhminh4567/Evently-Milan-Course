using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand(string TicketTypeId, decimal Price) : ICommand;
