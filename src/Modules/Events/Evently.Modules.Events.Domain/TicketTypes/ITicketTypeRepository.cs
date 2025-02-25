using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.TicketTypes;

public interface ITicketTypeRepository: IBaseRepository<TicketType>
{
    Task<TicketType?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string eventId, CancellationToken cancellationToken = default);

    void Insert(TicketType ticketType);
}
