using Evently.Common.Infrastructure.EventBuses;
using Evently.Common.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Ticketing.Api.Extensions;
using Evently.Ticketing.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;
using Evently.Common.Application;
using Evently.Common.Presentation.Endpoints;
using Evently.Ticketing.Api.OpenTelemetry;
namespace Evently.Ticketing.Api;
public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        builder.Services.AddSerilog((sp, config) =>
        {
            config.ReadFrom.Configuration(builder.Configuration);
        }, false, false);
        builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();
        builder.Services.AddScoped<LogContextTraceLoggingMiddleware>();
        builder.Services.AddProblemDetails();
        // --------------------------- Register commmon project first ---------------------------//
        // include setting up some services like event bus, consumer from other modules, 
        // after this will register other modules later
        string rabbitMqHost = builder.Configuration.GetConnectionString("Queue");
        ArgumentException.ThrowIfNullOrEmpty(rabbitMqHost);
        var rabbitMqSettings = new RabbitMqSettings()
        {
            Host = rabbitMqHost,
        };


        builder.Services.AddApplication(
            [
                    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly
            ]);
        builder.Services.AddInfrastructure(builder.Configuration, rabbitMqSettings,DiagnosticsConfig.ServiceName,
            [
                 TicketingModule.ConfigureConsumers,
    ]);

        //--------------------------------------------------------------------------------------------------------------------------------//
        builder.Configuration.AddModulesAppsettings(["ticketing"]);

        builder.Services.AddTicketingModule(builder.Configuration);

        // add appsettings of modules

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigrations();
        }
        // the position of useSerilogRequestLoggin() does have impact, correctly placing will log request process time correectly, 
        // more efficent and less noise ( or unecessary log ,like useStaticFile() handler )

        // -----------------------------------Add tracing and logging context to Serilog for tracing to Jaeger and Otlp--------------------------------------------//
        app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        app.UseSerilogRequestLogging();
        //app.UseExceptionHandler("/error");
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapEndpoints();

        app.Run();
    }
}
