using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal class GetTicketType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types/{id}", async (string id, ISender sender) =>
        {
            Result<TicketTypeResponse> result = await sender.Send(new GetTicketTypeQuery(id));

            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.TicketTypes);
    }
}
