using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.TicketTypes;

internal sealed class TicketTypeRepository : BaseRepository<TicketType>, ITicketTypeRepository
{
    public TicketTypeRepository(EventsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TicketType?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TicketTypes.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TicketTypes.AnyAsync(t => t.EventId == eventId, cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
		_dbContext.TicketTypes.Add(ticketType);
    }
}
