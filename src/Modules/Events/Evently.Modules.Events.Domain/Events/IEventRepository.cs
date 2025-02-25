using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events;
public interface IEventRepository : IBaseRepository<Event>
{
    void Insert(Event @event);
}
