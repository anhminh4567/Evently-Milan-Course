using System.Reflection;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Categories;
using Evently.Modules.Events.Presentation.Events;
using Evently.Modules.Events.Presentation.TicketTypes;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    ///////////////////////////
    // this method is no longer needed
    // replaced by automatci endpoint registration through assembly reference
    // it is registerd in Evently.Modules.Events.Infrastructure
    // the method do this is implemented in Evently.Common.Presentation
    ///////////////////////////

    //public static void MapEndpoints(IEndpointRouteBuilder app)
    //{
    //    EventEndpoints.MapEndpoints(app);
    //    TicketTypeEndpoints.MapEndpoints(app);
     
    //    CategoryEndpoints.MapEndpoints(app);
    //}

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // this is automatic enpoint registration, very cool stuff, check the implementation
        // currently this is registering all endpotn as transient service, then later register
        services.AddEndpoints(Evently.Modules.Events.Presentation.MetaClass.Assembly);
        //
        services.AddInfrastructure(configuration);
        return services;
    }
    private static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;
        services.AddDbContext<EventsDbContext>((sp, options) =>
        {
            // this is registered in the ---Evently.Common.Infrastructure---
            var domainEventInterceptors = sp.GetRequiredService<InsertOutboxMessageEventsInterceptor>();
            options
            .UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events)
            ).AddInterceptors(domainEventInterceptors);
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

		services.AddScoped<IEventRepository, EventRepository>();
		services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();

        //services.AddScoped<IEventsApi,EventsApi>();
		return services;
    }
}
