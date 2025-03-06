using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Events;

internal sealed class TicketTypeRepository : BaseRepository<TicketType>, ITicketTypeRepository
{
    private TicketingDbContext _context => _dbContext as TicketingDbContext;
    public TicketTypeRepository(TicketingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TicketType?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.TicketTypes.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TicketType?> GetWithLockAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context
            .TicketTypes
            .FromSql(
                $"""
                SELECT "Id", "EventId", "Name", "Price", "Currency", "Quantity", "AvailableQuantity"
                FROM ticketing."TicketTypes"
                WHERE "Id" = {id}
                FOR UPDATE NOWAIT
                """)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<TicketType> ticketTypes)
    {
        _context.TicketTypes.AddRange(ticketTypes);
    }
}
