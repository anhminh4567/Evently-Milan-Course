using System.Diagnostics.Eventing.Reader;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

public class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    // we define the state of the saga
    public State CancellationStarted { get; private set; }
    public State PaymentRefunded { get; private set; }
    public State TicketArchived { get; private set; }

    // we define what are the event that our SAGA is going to handle
    public Event<EventCanceledIntegrationEvent> EventCanceled { get; private set; }
    //Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;
    // above is the place where the EventPaymentRefunded Event is published, which will be handled by this SAGA
    // through mass transit
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentRefunded { get; private set; }
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketArchived { get; private set; }
    public Event EventCancellationCompleted { get; private set; }

    public CancelEventSaga()
    {
        // all of these event have a corrlateid by the Eventid
        // we use this to correlate which SAGA instance it belongs to
        Event(() => EventCanceled, c => c.CorrelateById(m =>  Guid.Parse(m.Message.EventId)));
        Event(() => EventPaymentRefunded, c => c.CorrelateById(m => Guid.Parse(m.Message.EventId)));
        Event(() => EventTicketArchived, c => c.CorrelateById(m => Guid.Parse(m.Message.EventId)));

        // from the document, this is just telling saga to use this property for state tracking
        InstanceState(s => s.CurrentState);

        Initially(
            When(EventCanceled)
            .Publish(context => new EventCancellationStartedIntegrationEvent(
                context.Message.Id,
                context.Message.OccurredOnUtc,
                context.Message.EventId)
                )
            .TransitionTo(CancellationStarted)
            );
        // --------------------------- thise group of events are to translatee event from one state to another ---------------------------
        // go to onenote to see the state flow
        // from started --> payment refunded --> ticket archived
        //                  PaymentRefunded --> TicketArchived
        //                  TicketArchived-- > PaymentRefunded
        During(CancellationStarted,
            When(EventPaymentRefunded).TransitionTo(PaymentRefunded),
            When(EventTicketArchived).TransitionTo(TicketArchived)
            );

        During(PaymentRefunded,
            When(EventTicketArchived).TransitionTo(TicketArchived));

        During(TicketArchived,
            When(EventPaymentRefunded).TransitionTo(PaymentRefunded));
        // ------------------------------------------------------------------------------------------------------------------------------------

        // -----------------------------------Composite Events--------------------------------------------------------------
        // this mean this event only PUBLISH when both of the process of both event is completed
        // ( EventPaymentRefunded and EventTicketArchived )
        CompositeEvent(
            () => EventCancellationCompleted,
            state => state.CancellationCompletedStatus,
            EventPaymentRefunded,EventTicketArchived
            );
        // During any state, when this event happened, then publish new IE
        DuringAny( 
            When(EventCancellationCompleted)
            .Publish(context => new EventCancellationCompletedIntegrationEvent(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow,
                context.Saga.CorrelationId.ToString() // this is also the eventId
                )
            ).Finalize());
    }
}

