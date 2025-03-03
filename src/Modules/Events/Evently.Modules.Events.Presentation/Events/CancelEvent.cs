using Evently.Modules.Events.Application.Events.CancelEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal class CancelEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id}/cancel", async (string id, ISender sender) =>
        {
            Result result = await sender.Send(new CancelEventCommand(id));

            return result.Match(Results.NoContent, Common.Presentation.ApiResults.ApiResults.Problem);
        }).RequireAuthorization()
        .WithTags(Tags.Events);
    }
}
