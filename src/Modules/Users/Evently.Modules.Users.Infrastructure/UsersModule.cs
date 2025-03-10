using Evently.Common.Application.Authorization;
using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Authorization;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using Evently.Modules.Users.Infrastructure.Inbox;
using Evently.Modules.Users.Infrastructure.Outbox;
using Evently.Modules.Users.Infrastructure.Users;
using MassTransit.Configuration;
using MassTransit.Futures.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        // -------------------------- register handler after remove mediator as the main domain Event consumer------------------------------
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        
        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

        // act as middlware for OUTBOUT request ( middleware is for INBOUN requets) 
        services.AddTransient<KeyCloakAuthDelegatingHandler>();


        // -------------------------- register KEYCLOAK client------------------------------
        // -------------------- with preconfigured http client -----------------------------
        // ----------------- and outbound requeest handler registerd above -----------------
        services.AddHttpClient<KeyCloakClient>((sp,httpClient) =>
        {
            KeyCloakOptions options = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.AdminUrl);
        }).AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();
        // -------------------------- register KEYCLOAK client------------------------------

        // add PermissionService impl IPermissionService (Evently.Common.Application)
        services.AddScoped<IPermissionService, PermissionService>();


        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        services.AddDbContext<UsersDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageEventsInterceptor>())
                );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();

        // ------------------------add QUARTZ BG job ------------------------//
        services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
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

