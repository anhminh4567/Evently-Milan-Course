namespace Evently.Modules.Ticketing.Application.Tickets.GetTicket;

public sealed record TicketResponse(
    string Id,
    string CustomerId,
    string OrderId,
    string EventId,
    string TicketTypeId,
    string Code,
    DateTime CreatedAtUtc);
