using Evently.Gateway.Authentication;
using Evently.Gateway.Middlewares;
using Evently.Gateway.OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
// Add services to the container.
builder.Services.AddSerilog((sp, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
}, false, false);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.Services.AddControllers();
builder.Services.AddScoped<LogContextTraceLoggingMiddleware>();

//------------------------------- OpenTelemetry SERVICE -------------------------------//
builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("Yarp.ReverseProxy");
        tracing.AddOtlpExporter();
    });

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureOptions<JwtBearerConfigureOptions>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<LogContextTraceLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();
