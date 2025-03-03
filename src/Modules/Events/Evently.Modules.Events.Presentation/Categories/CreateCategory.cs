using Evently.Modules.Events.Application.Categories.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (CreateCategoryRequest request, ISender sender) =>
        {
            Result<string> result = await sender.Send(new CreateCategoryCommand(request.Name));

            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }

    internal sealed class CreateCategoryRequest
	{
        public string Name { get; init; }
    }
}
