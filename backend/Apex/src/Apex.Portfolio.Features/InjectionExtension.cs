using Apex.Portfolio.Factories;
using Apex.Shared.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apex.Shared.Core.Shared;
using BondsDetails = Apex.Portfolio.Features.Modules.Bonds.Details;
using BondsOperations = Apex.Portfolio.Features.Modules.Bonds.Operations;
using InstrumentMetadataDetails = Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

namespace Apex.Portfolio.Features;

public static class InjectionExtension
{
    public static void AddFeaturesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidationRunner, ValidationRunner>();

        services.AddSharedDependencies();

        RegisterMappers(services);
        RegisterBonds(services);
        RegisterInstrumentMetadata(services);
    }

    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapperFactory>(sp =>
        {
            var factory = new MapperFactory();

            factory.RegisterMapper(() => new BondsDetails.BondMapper());
            factory.RegisterMapper(() => new BondsDetails.BondCreateMapper());
            factory.RegisterMapper(() => new BondsDetails.BondUpdateMapper());
            factory.RegisterMapper(() => new BondsDetails.BondListMapper());
            factory.RegisterMapper(() => new BondsOperations.BondOperationMapper());
            factory.RegisterMapper(() => new BondsOperations.BondOperationCreateMapper());
            factory.RegisterMapper(() => new BondsOperations.BondOperationUpdateMapper());
            factory.RegisterMapper(() => new BondsOperations.BondOperationListMapper());
            factory.RegisterMapper(() => new InstrumentMetadataDetails.InstrumentMetadataMapper());
            factory.RegisterMapper(() => new InstrumentMetadataDetails.InstrumentMetadataCreateMapper());
            factory.RegisterMapper(() => new InstrumentMetadataDetails.InstrumentMetadataUpdateMapper());
            factory.RegisterMapper(() => new InstrumentMetadataDetails.InstrumentMetadataListMapper());

            return factory;
        });
    }

    private static void RegisterBonds(IServiceCollection services)
    {
        services.AddScoped<BondsDetails.GetHandler>();
        services.AddScoped<BondsDetails.CreateHandler>();
        services.AddScoped<BondsDetails.UpdateHandler>();
        services.AddScoped<BondsDetails.GetListHandler>();

        services.AddScoped<BondsOperations.GetHandler>();
        services.AddScoped<BondsOperations.CreateHandler>();
        services.AddScoped<BondsOperations.UpdateHandler>();
        services.AddScoped<BondsOperations.DeleteHandler>();
        services.AddScoped<BondsOperations.GetListHandler>();
    }

    private static void RegisterInstrumentMetadata(IServiceCollection services)
    {
        services.AddScoped<InstrumentMetadataDetails.GetHandler>();
        services.AddScoped<InstrumentMetadataDetails.CreateHandler>();
        services.AddScoped<InstrumentMetadataDetails.UpdateHandler>();
        services.AddScoped<InstrumentMetadataDetails.DeleteHandler>();
        services.AddScoped<InstrumentMetadataDetails.GetListHandler>();
    }
}
