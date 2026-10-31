using Apex.Shared.Core.Shared;
using Apex.Users.Features;
using Apex.Users.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddSharedDependencies();
    builder.Services.AddControllers();
    builder.Services.AddPersistenceDependencies(builder.Configuration);
    builder.Services.AddFeaturesDependencies(builder.Configuration);

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
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]!)),
                ValidateLifetime = true,
                NameClaimType = "sub",
                RoleClaimType = "role"
            };
        });

    builder.Services.AddAuthorization();
}

static void ConfigureApplication(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}