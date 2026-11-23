using Apex.Portfolio.Features.IntegrationTests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Apex.Shared.Core.Shared;
using Apex.Portfolio.Features;
using Apex.Portfolio.Persistance;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddSharedControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSharedDependencies();
    builder.Services.AddPersistenceDependencies(builder.Configuration);
    builder.Services.AddFeaturesDependencies(builder.Configuration);
    builder.Services.AddSharedCors();
    builder.Services.AddSharedSwagger("Apex.Portfolio API", "Apex.Portfolio API - integration test host");

    builder.Services
        .AddAuthentication(TestAuthHandler.SchemeName)
        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
    builder.Services.AddAuthorization();
}

static void ConfigureApplication(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors();
}
