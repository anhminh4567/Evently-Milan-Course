using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Clock;
using Evently.Common.Infrastructure.Data;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;
        NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.AddSingleton(dataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
        return services;
    }
}
