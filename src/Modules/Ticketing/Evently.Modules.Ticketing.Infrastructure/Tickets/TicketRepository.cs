using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Tickets;

internal sealed class TicketRepository: BaseRepository<Ticket> , ITicketRepository
{
    private TicketingDbContext _context => _dbContext as TicketingDbContext;
    public TicketRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Ticket>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tickets.Where(t => t.EventId == @event.Id).ToListAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<Ticket> tickets)
    {
        _context.Tickets.AddRange(tickets);
    }
}
