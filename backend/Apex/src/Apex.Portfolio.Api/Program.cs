using System.Security.Cryptography;
using Apex.Portfolio.Features;
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
    builder.Services.AddSharedSwagger();

    var publicKeyPem = builder.Configuration["Authentication:PublicKey"];
    ArgumentException.ThrowIfNullOrWhiteSpace(publicKeyPem);

    using var rsa = RSA.Create();
    rsa.ImportFromPem(publicKeyPem);
    var signingKey = new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters: false));
    if (signingKey.KeySize < 2048)
        throw new InvalidOperationException("Authentication:PublicKey must be an RSA key of at least 2048 bits");

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
