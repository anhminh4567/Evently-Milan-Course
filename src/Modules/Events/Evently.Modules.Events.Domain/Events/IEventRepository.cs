using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Events;
public interface IEventRepository : IBaseRepository<Event>
{
    void Insert(Event @event);
}
