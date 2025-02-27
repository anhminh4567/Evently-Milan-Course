using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.PublicApi;
using Evently.Modules.Ticketing.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

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
               .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services.AddSingleton<CartService>();
        services.AddScoped<ITicketingApi,TicketingApi>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());
        return services;
    }
}
