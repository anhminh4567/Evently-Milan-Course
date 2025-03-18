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
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public const string ServiceName = "Evently.Api";
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        RabbitMqSettings rabbitMqSettings,
        Action<IRegistrationConfigurator,string>[] eventConsumerRegistration)
	{
		string databaseConnectionString = configuration.GetConnectionString("Database")!;
		string cacheConnectionString = configuration.GetConnectionString("CachingService");
        //string messaggeQueueConnectionString = configuration.GetConnectionString("Queue");
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
		services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

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
            // instance id here simply mean adding the extra identifider to the queue and handler name
            // since whe moving to microservice, shit get complicateed
            string instanceId = ServiceName.ToLowerInvariant().Replace(".", "-");

            foreach (var moduleConsumerRegister in eventConsumerRegistration)
			{
				moduleConsumerRegister(config,instanceId);
			}
            //config.UsingInMemory((ctx, cfg) =>
            //{
            //	cfg.ConfigureEndpoints(ctx);
            //});
            config.SetKebabCaseEndpointNameFormatter();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqSettings.Host),config =>
                {
                    config.Username(rabbitMqSettings.Username);
                    config.Password(rabbitMqSettings.Password);
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });
		//------------------------------- Event buss section -------------------------------



		//------------------------------- QUARTZ for BG Job -------------------------------//
		services.AddQuartz(configurator =>
		{
			//---------------this some high stuff shit--------------
			// since we run test in the integration test, which might spinup multiple instance of this applicatio, API
			// so quartz , without changing name and id, will spawn many scheduler that have the same id, 
			// so quartz will throw error and fail

			var scheduler = Guid.NewGuid();
			configurator.SchedulerId = $"default-id-{scheduler}";
			configurator.SchedulerName = $"default-name-{scheduler}";
		});
		services.AddQuartzHostedService(options =>
		{
			options.WaitForJobsToComplete = true;
		});
		//------------------------------- QUARTZ for BG Job -------------------------------//

		//----------------------------------------------------------------------------------------------------------------------------------------------------------


		//------------------------------- OpenTelemetry SERVICE -------------------------------//
		services
			.AddOpenTelemetry()
			.ConfigureResource(resource => resource.AddService(ServiceName))
			.WithTracing(tracing =>
			{
				tracing
					.AddAspNetCoreInstrumentation(config =>
                    {
                        
                    })
					.AddHttpClientInstrumentation()
					.AddEntityFrameworkCoreInstrumentation()
					.AddRedisInstrumentation(connectionMultiplexer)
					.AddNpgsql(option =>
                    {
                        //option.EnableConnectionLevelAttributes = true;
                        //option.EnableStatementLevelAttributes = true;
                    })
					.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

				tracing.AddOtlpExporter();
			});

		// Enable detailed logging for OpenTelemetry and Redis

		return services;
	}
}
