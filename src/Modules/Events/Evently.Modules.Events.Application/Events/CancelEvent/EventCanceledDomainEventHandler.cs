using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class EventCanceledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // this Integration Event is catched by the SAGA CancelEventSaga (in this module at Presentation)
        await eventBus.PublishAsync(
            new EventCanceledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccuredTimeUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
