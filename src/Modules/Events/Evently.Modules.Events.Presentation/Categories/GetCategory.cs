using Evently.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id}", async (string id, ISender sender) =>
        {
            Result<CategoryResponse> result = await sender.Send(new GetCategoryQuery(id));
            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
