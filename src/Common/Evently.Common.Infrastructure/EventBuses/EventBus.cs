using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Application.EventBus;
using MassTransit;

namespace Evently.Common.Infrastructure.EventBuses;
public class EventBus : IEventBus
{
    private readonly IBus _bus;

    public EventBus(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync<T>(T integrationEvent, CancellationToken token = default) where T : IIntegrationEvent
    {
        return _bus.Publish(integrationEvent, token);
    }
}
