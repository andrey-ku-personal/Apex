using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using Apex.Portfolio.Features.Modules.Bonds.Details.Services;
using Apex.Portfolio.Features.Modules.Bonds.List.Models;
using Apex.Portfolio.Features.Modules.Bonds.List.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;

namespace Apex.Portfolio.Features;

public static class InjectionExtension
{
    public static void AddFeaturesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidationRunner, ValidationRunner>();

        RegisterMappers(services);
        RegisterInstrumentMetadata(services);
        RegisterBonds(services);
    }

    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapperFactory>(sp =>
        {
            var factory = new MapperFactory();

            factory.RegisterMapper(() => new InstrumentMetadataListModelMapper());
            factory.RegisterMapper(() => new InstrumentMetadataModelMapper());
            factory.RegisterMapper(() => new BondModelMapper());
            factory.RegisterMapper(() => new BondListModelMapper());

            return factory;
        });
    }

    private static void RegisterInstrumentMetadata(IServiceCollection services)
    {
        services.AddScoped<InstrumentMetadataService>();
        services.AddScoped<IInstrumentMetadataService>(sp =>
        {
            var service = sp.GetRequiredService<InstrumentMetadataService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new InstrumentMetadataDecorator(service, validator);
        });

        services.AddScoped<InstrumentMetadataListService>();
        services.AddScoped<IInstrumentMetadataListService>(sp => sp.GetRequiredService<InstrumentMetadataListService>());
    }

    private static void RegisterBonds(IServiceCollection services)
    {
        services.AddScoped<BondService>();
        services.AddScoped<IBondService>(sp =>
        {
            var service = sp.GetRequiredService<BondService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new BondDecorator(service, validator);
        });

        services.AddScoped<BondListService>();
        services.AddScoped<IBondListService>(sp => sp.GetRequiredService<BondListService>());
    }
}
