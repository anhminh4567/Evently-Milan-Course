using Evently.Api.Extensions;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Events.Application;
using Evently.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.CustomOperationIds(type => type.ToString());
});


builder.Services.AddApplication([Evently.Modules.Events.Application.MetaClass.EventApplicationAssembly]);
builder.Services.AddInfrastructure(builder.Configuration);
// add appsettings of modules
builder.Configuration.AddModulesAppsettings(["events"]);

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.Run();
