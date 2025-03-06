using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Tickets;

internal sealed class TicketRepository:  BaseRepository<Ticket>, ITicketRepository
{
    private AttendanceDbContext context => _dbContext as AttendanceDbContext;
    public TicketRepository(AttendanceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Ticket?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Tickets.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public void Insert(Ticket ticket)
    {
        context.Tickets.Add(ticket);
    }
}
