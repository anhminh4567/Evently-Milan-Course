namespace Evently.Modules.Events.Application.Events.GetEvent;

public sealed record TicketTypeResponse(
    string TicketTypeId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
