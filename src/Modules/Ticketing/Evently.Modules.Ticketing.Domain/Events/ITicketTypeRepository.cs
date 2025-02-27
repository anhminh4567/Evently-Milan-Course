using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events;

public interface ITicketTypeRepository : IBaseRepository<TicketType>
{
    Task<TicketType?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<TicketType?> GetWithLockAsync(string id, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<TicketType> ticketTypes);
}
