using Evently.Common.Domain;
using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Attendees;

internal sealed class AttendeeRepository: BaseRepository<Attendee>, IAttendeeRepository
{
    private AttendanceDbContext _context => _dbContext as AttendanceDbContext;
    public AttendeeRepository(AttendanceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Attendee?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Attendees.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Attendee attendee)
    {
        _context.Attendees.Add(attendee);
    }
}
