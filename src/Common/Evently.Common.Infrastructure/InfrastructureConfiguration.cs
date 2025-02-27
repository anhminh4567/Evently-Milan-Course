using Evently.Common.Application.Caching;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Clock;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.Outbox;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;
        string cacheConnectionString = configuration.GetConnectionString("CachingService");
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
            /// 1st method to register
            //opt.Configuration = cacheConnectionString;
            //opt.InstanceName = "instancename";
            /// 2nd method to register
            opt.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });

        //register interceptors
        services.AddSingleton<PublishDomainEventsInterceptor>();
        return services;
    }
}
