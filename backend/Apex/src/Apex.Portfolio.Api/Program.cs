using Apex.Portfolio.Features;
using Apex.Portfolio.Features.Security;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Extensions;
using Apex.Shared.Core.Middleware;
using Apex.Shared.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    builder.Services.AddSharedControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSharedDependencies();
    builder.Services.AddPersistenceDependencies(builder.Configuration);
    builder.Services.AddFeaturesDependencies(builder.Configuration);
    builder.Services.AddSharedCors();
    builder.Services.AddSharedSwagger("Apex.Portfolio API", "Apex.Portfolio API - bonds and bond operations");

    var publicKey = builder.Configuration["Authentication:PublicKey"];
    ArgumentException.ThrowIfNullOrWhiteSpace(publicKey);

    var signingKey = TokenService.CreateValidationKey(publicKey);

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Authentication:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                NameClaimType = "sub",
                RoleClaimType = "role"
            };
        });

    builder.Services.AddAuthorization();
}

static void ConfigureApplication(WebApplication app)
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSharedCors();
    app.UseSharedSwagger();
}
