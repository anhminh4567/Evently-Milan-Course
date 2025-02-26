using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
    ///////////////////////////
    // this method is no longer needed
    // replaced by automatci endpoint registration through assembly reference
    // it is registerd in Evently.Modules.Events.Infrastructure
    // the method do this is implemented in Evently.Common.Presentation
    ///////////////////////////


    //public static void MapEndpoints(IEndpointRouteBuilder app)
    //{
    //    CancelEvent.MapEndpoint(app);
    //    CreateEvent.MapEndpoint(app);
    //    GetEvent.MapEndpoint(app);
    //    GetEvents.MapEndpoint(app);
    //    PublishEvent.MapEndpoint(app);
    //    RescheduleEvent.MapEndpoint(app);
    //    SearchEvents.MapEndpoint(app);
    //}
}
