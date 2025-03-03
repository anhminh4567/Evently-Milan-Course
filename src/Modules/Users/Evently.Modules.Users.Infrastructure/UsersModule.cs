using Evently.Common.Application.Authorization;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Authorization;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using Evently.Modules.Users.Infrastructure.Users;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
                );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();

        //public api
        //services.AddScoped<IUserApi, UserApi>();
    }
}
