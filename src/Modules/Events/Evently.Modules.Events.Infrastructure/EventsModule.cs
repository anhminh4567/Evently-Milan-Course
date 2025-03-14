using System.Reflection;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
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
using Evently.Modules.Events.Infrastructure.Inbox;
using Evently.Modules.Events.Infrastructure.Outbox;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Categories;
using Evently.Modules.Events.Presentation.Events;
using Evently.Modules.Events.Presentation.Events.CancelEventSaga;
using Evently.Modules.Events.Presentation.TicketTypes;
using FluentValidation;
using MassTransit;
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
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);
        // this is automatic enpoint registration, very cool stuff, check the implementation
        // currently this is registering all endpotn as transient service, then later register
        services.AddEndpoints(Evently.Modules.Events.Presentation.MetaClass.Assembly);
        //
        
        return services;
    }
    public static Action<IRegistrationConfigurator> ConfigureConsumers(IConfiguration configuration)
    {
        // redis persitance, from Common.Infrastructure
        // you can change to EFCore or RabbitMQ later
        return registrationConfigurator =>  
                registrationConfigurator.AddSagaStateMachine<CancelEventSaga, CancelEventState>()
                .RedisRepository(configuration.GetConnectionString("CachingService"));
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

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Events:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
        return services;
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.MetaClass.EventApplicationAssembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);
            // we get the type of domainEvent from the Handlers we get from current assembly 
            // since each handler implemennt DomainEventHandler<T> : IDomainEventHandler<T> : IDomainEvent
            // ==> we can get the argument <T> (generic argument), easily, as it is the only one in the list ( .GetGenericArguments().Single() )
            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();
            // after getting the type of the domainEvent of the handler, we create a new Type , which
            // replace the DomainEventHandler<> , with the new IdempotentDomainEventHandler<>
            // By taking the <T> from DomainEventHandler<T> to IdempotentDomainEventHandler<T>
            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);
            // closedIdempotentHandler ==> means this generic is specific to a specifit type, not open ( open means T , like hey, this 
            // thing might fit many type


            // ---------------------------------- this method Decorate() is form Scrutor library ( in  ---- Common.Infastructure ---- )
            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.MetaClass.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}
