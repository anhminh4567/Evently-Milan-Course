using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Event @event);
}
