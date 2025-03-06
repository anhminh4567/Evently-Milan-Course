namespace Evently.Modules.Attendance.Domain.Tickets;

public interface ITicketRepository
{
    Task<Ticket?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Ticket ticket);
}
