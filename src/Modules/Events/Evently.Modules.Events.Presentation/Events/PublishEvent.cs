using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Events.PublishEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class PublishEvent //: IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id}/publish", async (string id, ISender sender) =>
        {
            Result result = await sender.Send(new PublishEventCommand(id));

            return result.Match(Results.NoContent, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }
}
