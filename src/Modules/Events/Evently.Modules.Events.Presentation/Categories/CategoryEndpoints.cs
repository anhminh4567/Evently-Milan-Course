using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public static class CategoryEndpoints
{
    ///////////////////////////
    // this method is no longer needed
    // replaced by automatci endpoint registration through assembly reference
    // it is registerd in Evently.Modules.Events.Infrastructure
    // the method do this is implemented in Evently.Common.Presentation
    ///////////////////////////



    //public static void MapEndpoints(IEndpointRouteBuilder app)
    //{
    //    ArchiveCategory.MapEndpoint(app);
    //    CreateCategory.MapEndpoint(app);
    //    GetCategory.MapEndpoint(app);
    //    GetCategories.MapEndpoint(app);
    //    UpdateCategory.MapEndpoint(app);
    //}

}
