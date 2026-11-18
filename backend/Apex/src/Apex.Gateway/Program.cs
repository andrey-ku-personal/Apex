using Apex.Shared.Core.Extensions;
using Apex.Shared.Core.Middleware;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    RegisterServices(builder);

    var app = builder.Build();

    ConfigureApplication(app);

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    logger.LogAppBye();
    LogManager.Shutdown();
}

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
}

static void ConfigureApplication(WebApplication app)
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.MapReverseProxy();
}