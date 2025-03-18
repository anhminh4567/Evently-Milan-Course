using Evently.Common.Application.Authorization;
using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure.Authentication;
using Evently.Modules.Ticketing.Infrastructure.Authorization;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.Events;
using Evently.Modules.Ticketing.Infrastructure.Inbox;
using Evently.Modules.Ticketing.Infrastructure.Orders;
using Evently.Modules.Ticketing.Infrastructure.Outbox;
using Evently.Modules.Ticketing.Infrastructure.Payments;
using Evently.Modules.Ticketing.Infrastructure.Tickets;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    // ---------------------------- ADD CONSUMER ---------------------------- //
    // ---------------------------- ADD CONSUMER ---------------------------- //

    // this function will be passed to  -----------------Common.Infrastructure----------------- to register the consumer
    // through -----------------Event.Api----------------- ( since event.APi reference this, and this refernce the Common
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId)
    {
        //        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>().Endpoint(c => c.InstanceId = instanceId);
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserProfileUpdatedIntegrationEvent>>().Endpoint(c => c.InstanceId = instanceId);
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<EventPublishedIntegrationEvent>>().Endpoint(c => c.InstanceId = instanceId);
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<TicketTypePriceChangedIntegrationEvent>>().Endpoint(c => c.InstanceId = instanceId);
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<EventCancellationStartedIntegrationEvent>>().Endpoint(c => c.InstanceId = instanceId);

    }

    // ---------------------------- ADD CONSUMER ---------------------------- //
    // ---------------------------- ADD CONSUMER ---------------------------- //

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Console.Write(configuration.ToString());
        // Will implement this later.
        string dbConnectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<TicketingDbContext>((sp, options) =>
           options
               .UseNpgsql(
                   dbConnectionString,
                   npgsqlOptions => npgsqlOptions
                       .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing))
               .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageEventsInterceptor>()));


        //services.AddScoped<ITicketingApi,TicketingApi>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddSingleton<CartService>();
        services.AddSingleton<IPaymentService, PaymentService>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());

        services.AddScoped<ICustomerContext, CustomerContext>();

        services.Configure<OutboxOptions>(configuration.GetSection("Ticketing:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Ticketing:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();

        services.AddScoped<IPermissionService, PermissionService>();
        return services;
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
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
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
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
