using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Api.Middlewares;
using Evently.Api.OpenTelemetry;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Infrastructure.EventBuses;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

public partial class Program
{
    private static void Main(string[] args)
    {
        // this is used to start a log before service provider, to log state of application
        // service provider addSerilog() will override this so no worry
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        //builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));



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
        //builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        // --------------------------- Register commmon project first ---------------------------//
        // include setting up some services like event bus, consumer from other modules, 
        // after this will register other modules later
        var rabbitMqSettings = new RabbitMqSettings()
        {
            Host = builder.Configuration.GetConnectionString("Queue"),
        };


        builder.Services.AddApplication(
            [Evently.Modules.Events.Application.MetaClass.EventApplicationAssembly,
            Evently.Modules.Users.Application.AssemblyReference.Assembly,
            Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
            Evently.Modules.Attendance.Application.AssemblyReference.Assembly,
         ]);
        builder.Services.AddInfrastructure(builder.Configuration, rabbitMqSettings, [
                EventsModule.ConfigureConsumers(builder.Configuration), // config consumer delegate from EventsModule
                TicketingModule.ConfigureConsumers, // config consumer delegate from TicketingModule
                AttendanceModule.ConfigureConsumers,
         ]);
        // --------------------------- Register commmon project first ---------------------------//

        //--------------------------------------------------------------------------------------------------------------------------------//
        builder.Configuration.AddModulesAppsettings(["events", "users", "attendance", "ticketing"]);

        builder.Services.AddEventsModule(builder.Configuration);
        builder.Services.AddUsersModule(builder.Configuration);
        builder.Services.AddTicketingModule(builder.Configuration);
        builder.Services.AddAttendanceModule(builder.Configuration);

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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ||                                                                                                             ||
        // ||   AUTO MATIC REGISTRATION ENDPOINT                                                                          ||
        // ||   REQUIRE AddEndpoints() in ------ Evently.Common.Presentation ------ to be called in Evently.Modules.<>.Presentation     ||
        // ||                                                                                                             ||
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //** Old Registration
        //EventsModule.MapEndpoints(app);
        // ** new automatic stuff
        // from ----- Evently.Common.Presentation.Endpoints ------
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapEndpoints();

        app.Run();
    }
}
