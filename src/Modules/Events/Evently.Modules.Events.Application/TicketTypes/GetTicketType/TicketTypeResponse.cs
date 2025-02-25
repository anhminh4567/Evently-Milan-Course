namespace Evently.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record TicketTypeResponse(
    string Id,
    string EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
