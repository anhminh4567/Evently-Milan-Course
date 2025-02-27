using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Tickets;

public sealed class Ticket : Entity
{
    private Ticket()
    {
    }

    public string Id { get; private set; }

    public string CustomerId { get; private set; }

    public string OrderId { get; private set; }

    public string EventId { get; private set; }

    public string TicketTypeId { get; private set; }

    public string Code { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public bool Archived { get; private set; }

    public static Ticket Create(Order order, TicketType ticketType)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = order.CustomerId,
            OrderId = order.Id,
            EventId = ticketType.EventId,
            TicketTypeId = ticketType.Id,
            Code = $"tc_{Ulid.NewUlid()}",
            CreatedAtUtc = DateTime.UtcNow
        };

        ticket.Raise(new TicketCreatedDomainEvent(ticket.Id));

        return ticket;
    }

    public void Archive()
    {
        if (Archived)
        {
            return;
        }

        Archived = true;

        Raise(new TicketArchivedDomainEvent(Id, Code));
    }
}
