using Evently.Common.Application.Caching;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Authentication;
using Evently.Common.Infrastructure.Authorization;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Clock;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.EventBuses;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration , Action<IRegistrationConfigurator>[] eventConsumerRegistration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;
        string cacheConnectionString = configuration.GetConnectionString("CachingService");

        //------------------------------- Auth section -------------------------------
        services.AddAuthenticationInternal(configuration);
        //------------------------------- Auth section -------------------------------
        //------------------------------- Authorization section -------------------------------
        services.AddAuthorizationInternal();
        //------------------------------- Authorization section -------------------------------

        NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.AddSingleton(dataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));

        services.TryAddSingleton<ICacheService, CacheService>();
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString, config =>
        {
            // this only be places here for ONE PURPOSE ( SHOULD BE REMOVED IN PROD  )
            //  --------------------- FOR MIGRATION PURPOSE ----------------------------
            // without this migration  can't build project
            config.AbortOnConnectFail = false;
        });
        services.TryAddSingleton(connectionMultiplexer);
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });

        //register interceptors
        services.AddSingleton<InsertOutboxMessageEventsInterceptor>();


        //------------------------------- Event buss section -------------------------------
        // add Event bus
        services.TryAddSingleton<IEventBus, EventBus>();
        // add masstransit
        services.AddMassTransit(config =>
        {
            // consumer is not in this assembly
            // this is passed down from Event.Api
            foreach( var moduleConsumerRegister  in eventConsumerRegistration)
            {
                moduleConsumerRegister(config);
            }
            config.UsingInMemory((ctx,cfg) => 
            {
                cfg.ConfigureEndpoints(ctx);
            });
        });
        //------------------------------- Event buss section -------------------------------



        //------------------------------- QUARTZ for BG Job -------------------------------//
        services.AddQuartz();
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        //------------------------------- QUARTZ for BG Job -------------------------------//
        return services;
        
    }
}
