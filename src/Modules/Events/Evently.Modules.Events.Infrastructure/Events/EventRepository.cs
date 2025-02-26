using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Database;

namespace Evently.Modules.Events.Infrastructure.Events;

internal class EventRepository : BaseRepository<Event>, IEventRepository
{
    private EventsDbContext _context => _dbContext as EventsDbContext;
    public EventRepository(EventsDbContext dbContext) : base(dbContext)
    {
    }

    public void Insert(Event @event)
    {
        _context.Events.Add(@event);
    }
}
