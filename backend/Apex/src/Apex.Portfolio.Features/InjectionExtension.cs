using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Mapper;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Services;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;
using Apex.Portfolio.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

namespace Apex.Portfolio.Features;

public static class InjectionExtension
{
    public static void AddFeaturesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidationRunner, ValidationRunner>();

        services.AddSingleton<IMapperFactory>(sp =>
        {
            var factory = new MapperFactory();

            factory.RegisterMapper(() => new FinancialAttributeListModelMapper());
            factory.RegisterMapper(() => new FinancialAttributeModelMapper());

            return factory;
        });

        services.AddScoped<FinancialAttributeService>();
        services.AddScoped<IFinancialAttributeService>(sp =>
        {
            var service = sp.GetRequiredService<FinancialAttributeService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new FinancialAttributeDecorator(service, validator);
        });

        services.AddScoped<FinancialAttributeListService>();
        services.AddScoped<IFinancialAttributeListService>(sp => sp.GetRequiredService<FinancialAttributeListService>());
    }
}
