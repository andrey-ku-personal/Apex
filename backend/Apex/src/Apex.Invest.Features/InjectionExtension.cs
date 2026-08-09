using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Bonds.Details.Mapper;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Invest.Features.Modules.Bonds.List.Mapper;
using Apex.Invest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Invest.Features;

public static class InjectionExtension
{
    public static void AddFeaturesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidationRunner, ValidationRunner>();

        services.AddSingleton<IMapperFactory>(sp =>
        {
            var factory = new MapperFactory();

            factory.RegisterMapper(() => new BondDetailsMapper());
            factory.RegisterMapper(() => new BondListMapper());

            return factory;
        });

        services.AddScoped<BondDetailsService>();
        services.AddScoped<IBondDetailsService>(sp =>
        {
            var service = sp.GetRequiredService<BondDetailsService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new BondDetailsDecorator(service, validator);
        });
    }
}
