using Apex.Shared.Core.Shared;
using Apex.Invest.Features;
using Apex.Invest.Persistance;
using NLog;
using NLog.Web;
using Apex.Shared.Core.Middleware;
using Apex.Shared.Core.Extensions;

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
    builder.Services.AddSharedControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSharedDependencies();
    builder.Services.AddPersistenceDependencies(builder.Configuration);
    builder.Services.AddFeaturesDependencies(builder.Configuration);
    builder.Services.AddSharedCors();
    builder.Services.AddSharedSwagger();
}

static void ConfigureApplication(WebApplication app)
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSharedCors();
    app.UseSharedSwagger();
}