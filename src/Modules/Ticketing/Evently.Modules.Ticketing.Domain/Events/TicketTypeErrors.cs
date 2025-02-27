using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events;

public static class TicketTypeErrors
{
    public static Error NotFound(string ticketTypeId) =>
        Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");

    public static Error NotEnoughQuantity(decimal availableQuantity) =>
        Error.Problem(
            "TicketTypes.NotEnoughQuantity",
            $"The ticket type has {availableQuantity} quantity available");
}
