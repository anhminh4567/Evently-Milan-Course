using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Events.CreateEvent;

public sealed record CreateEventCommand(
    string EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    List<CreateEventCommand.TicketTypeRequest> TicketTypes) : ICommand
{
    public sealed record TicketTypeRequest(
        string TicketTypeId,
        string EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity);
}
