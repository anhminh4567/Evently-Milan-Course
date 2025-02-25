using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

public sealed record CreateTicketTypeCommand(
    string EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity) : ICommand<string>;
