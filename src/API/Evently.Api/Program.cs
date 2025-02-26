using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Api.Middlewares;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Events.Infrastructure;
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
builder.Services.AddApplication([Evently.Modules.Events.Application.MetaClass.EventApplicationAssembly]);
builder.Services.AddInfrastructure(builder.Configuration);
// add appsettings of modules

builder.Services.AddEventsModule(builder.Configuration);
builder.Configuration.AddModulesAppsettings(["events"]);

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
EventsModule.MapEndpoints(app);


app.Run();


