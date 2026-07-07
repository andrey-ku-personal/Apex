using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Apex.Shared.Core.Services;
using Apex.Shared.Core.Exceptions.Extensions;
using Apex.Shared.Core.Extensions;

namespace Apex.Shared.Core.Shared;

public static class InjectionExtension
{
    public static void AddSharedDependencies(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(AssemblyExtension.GetSolutionAssemblies());

        services.AddTransient<IDateTime, Services.DateTime>();
    }


    public static void AddSharedControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionHandlerAttribute>();
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(new DefaultNamingStrategy()));
            options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = false, OverrideSpecifiedNames = true } };
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.Formatting = Formatting.Indented;
        });
    }

    public static void AddSharedCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public static void AddSharedSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Apex.Invest API",
                Version = "v1",
                Description = "Apex.Invest API",
                Contact = new OpenApiContact
                {
                    Name = "Andrey"
                },
            });

            options.CustomSchemaIds(type => type.ToString());
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });



            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });

        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void UseSharedCors(this WebApplication app)
    {
        app.UseCors();
    }

    public static void UseSharedSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
