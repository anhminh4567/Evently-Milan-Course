using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public static class TicketTypeEndpoints
{
    ///////////////////////////
    // this method is no longer needed
    // replaced by automatci endpoint registration through assembly reference
    // it is registerd in Evently.Modules.Events.Infrastructure
    // the method do this is implemented in Evently.Common.Presentation
    ///////////////////////////

    //public static void MapEndpoints(IEndpointRouteBuilder app)
    //{
    //    ChangeTicketTypePrice.MapEndpoint(app);
    //    CreateTicketType.MapEndpoint(app);
    //    GetTicketType.MapEndpoint(app);
    //    GetTicketTypes.MapEndpoint(app);
    //}
}
