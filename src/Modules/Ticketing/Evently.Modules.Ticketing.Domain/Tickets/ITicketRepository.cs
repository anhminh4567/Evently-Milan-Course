using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Domain.Tickets;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<Ticket> tickets);
}
