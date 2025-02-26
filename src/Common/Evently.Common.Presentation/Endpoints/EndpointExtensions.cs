using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Common.Presentation.Endpoints;


// this function is crucial 
// this help automatically register Endpoints 
// not important as this is necessary for the application to run
// but rather a new method to conveniently register API endpoint
// instead of static class extension method in App
// but i still prefer extension, make things less confuse
public static class EndpointExtensions
{
    // AddEndpoints() 
    // Register endpoints WHICH IMPLEMENT IEndpoint interface
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        ServiceDescriptor[] serviceDescriptors = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            // the where above is select like this:
            //  - select all those not Interface or abstract class
            //  - select all those implement IEndpoint
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
    // MapEndpoins()
    // get the registered Endpoints classes above, take them , pass the WebApplication app , to register new endpoint 

    public static IApplicationBuilder MapEndpoints(this WebApplication app,RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
