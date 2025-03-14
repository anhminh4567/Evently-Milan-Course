using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;
public class CancelEventState : SagaStateMachineInstance, ISagaVersion
{
    // a unique value for all incoming messages for this saga
    // in our case we will use EventIdentifier as corrlation for our saga
    // from SagaStateMachineInstance Interface
    public Guid CorrelationId { get; set; }
    // from ISagaVersion Interface ( for optimisitc concurrency control )
    public int Version { get; set; }
    // track current sate of saga ( not from any library, from our own implementation )
    public string CurrentState { get; set; }
    public int CancellationCompletedStatus { get; set; }
}

