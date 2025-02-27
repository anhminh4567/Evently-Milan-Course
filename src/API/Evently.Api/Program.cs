using Evently.Api.Extensions;
using Evently.Api.Middlewares;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using Serilog;

// this is used to start a log before service provider, to log state of application
// service provider addSerilog() will override this so no worry
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    //opt.CustomOperationIds(type => type.ToString());
});

builder.Services.AddSerilog((sp, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
}, false, false);
builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApplication(
    [Evently.Modules.Events.Application.MetaClass.EventApplicationAssembly,
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
    ]);
builder.Services.AddInfrastructure(builder.Configuration);
// add appsettings of modules

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);
builder.Configuration.AddModulesAppsettings(["events", "users"]);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}
// the position of useSerilogRequestLoggin() does have impact, correctly placing will log request process time correectly, 
// more efficent and less noise ( or unecessary log ,like useStaticFile() handler )
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
app.MapEndpoints();

app.Run();


