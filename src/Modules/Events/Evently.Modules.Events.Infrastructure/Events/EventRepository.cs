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
    public EventRepository(EventsDbContext dbContext) : base(dbContext)
    {
    }

    public void Insert(Event @event)
    {
        _dbContext.Events.Add(@event);
    }
}
