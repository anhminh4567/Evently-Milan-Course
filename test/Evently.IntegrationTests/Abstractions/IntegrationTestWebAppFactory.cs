using Evently.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Evently.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Api.Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("evently")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    //private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
    //    .WithImage("quay.io/keycloak/keycloak:latest")
    //    .WithResourceMapping(
    //        new FileInfo("evently-realm-export.json"),
    //        new FileInfo("/opt/keycloak/data/import/realm.json"))
    //    .WithCommand("--import-realm")
    //    .Build();
    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
        .WithImage("quay.io/keycloak/keycloak:latest")
        .WithResourceMapping(
        new FileInfo("evently-realm-export.json"),
        new FileInfo("/opt/keycloak/data/import/realm.json"))
        .WithCommand("--import-realm")
        .WithName("test-keycloak-" + Guid.NewGuid().ToString())
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ConnectionStrings:CachingService", _redisContainer.GetConnectionString());

        Environment.SetEnvironmentVariable("Users:Outbox:IntervalSeconds","5");
        Environment.SetEnvironmentVariable("Users:Inbox:IntervalSeconds", "5");

        Environment.SetEnvironmentVariable("Ticketing:Outbox:IntervalSeconds", "5");
        Environment.SetEnvironmentVariable("Ticketing:Inbox:IntervalSeconds", "5");

        Environment.SetEnvironmentVariable("Events:Outbox:IntervalSeconds", "5");
        Environment.SetEnvironmentVariable("Events:Inbox:IntervalSeconds", "5");

        Environment.SetEnvironmentVariable("Attendance:Outbox:IntervalSeconds", "5");
        Environment.SetEnvironmentVariable("Attendance:Inbox:IntervalSeconds", "5");


        string keycloakAddress = _keycloakContainer.GetBaseAddress();
        string keyCloakRealmUrl = $"{keycloakAddress}realms/Evently";

        Environment.SetEnvironmentVariable(
            "Authentication:MetadataAddress",
            $"{keyCloakRealmUrl}/.well-known/openid-configuration");
        Environment.SetEnvironmentVariable(
            "Authentication:TokenValidationParameters:ValidIssuer",
            keyCloakRealmUrl);

        builder.ConfigureTestServices(services =>
        {
            services.Configure<KeyCloakOptions>(o =>
            {
                o.AdminUrl = $"{keycloakAddress}admin/realms/Evently/";
                o.TokenUrl = $"{keyCloakRealmUrl}/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {

        try
        {
            Console.WriteLine("Starting PostgreSQL container...");
            await _dbContainer.StartAsync();
            Console.WriteLine("PostgreSQL container started.");

            Console.WriteLine("Starting Redis container...");
            await _redisContainer.StartAsync();
            Console.WriteLine("Redis container started.");

            Console.WriteLine("Starting Keycloak container...");
            await _keycloakContainer.StartAsync();
            Console.WriteLine("Keycloak container started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting containers: {ex.Message}");
            throw;
        }
        //await _dbContainer.StartAsync();
        //await _redisContainer.StartAsync();
        //await _keycloakContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        try
        {
            Console.WriteLine("Stopping PostgreSQL container...");
            await _dbContainer.StopAsync();
            Console.WriteLine("PostgreSQL container stopped.");

            Console.WriteLine("Stopping Redis container...");
            await _redisContainer.StopAsync();
            Console.WriteLine("Redis container stopped.");

            Console.WriteLine("Stopping Keycloak container...");
            await _keycloakContainer.StopAsync();
            Console.WriteLine("Keycloak container stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping containers: {ex.Message}");
            throw;
        }
        //await _dbContainer.StopAsync();
        //await _redisContainer.StopAsync();
        //await _keycloakContainer.StopAsync();
    }
}
