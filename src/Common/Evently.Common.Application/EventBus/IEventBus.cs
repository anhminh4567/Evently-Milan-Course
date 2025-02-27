namespace Evently.Common.Application.EventBus;

// implemented in ----------------------- Common.Infrastructure -----------------------
public interface IEventBus
{
    Task PublishAsync<T>(T integrationEvent, CancellationToken token = default) 
         where T : IIntegrationEvent;

}

