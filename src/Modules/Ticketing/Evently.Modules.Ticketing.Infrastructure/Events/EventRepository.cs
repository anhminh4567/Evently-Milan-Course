using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Events;

internal sealed class EventRepository : BaseRepository<Event> , IEventRepository
{
    private TicketingDbContext _context => _dbContext as TicketingDbContext;
    public EventRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Event?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        _context.Events.Add(@event);
    }
}
