using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Attendees;

public interface IAttendeeRepository : IBaseRepository<Attendee>
{
    Task<Attendee?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Attendee attendee);
}
