using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Bonds.Details.Mapper;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Invest.Features.Modules.Bonds.List.Mapper;
using Apex.Invest.Features.Modules.Bonds.List.Services;
using Apex.Invest.Features.Modules.Deposits.Details.Mapper;
using Apex.Invest.Features.Modules.Deposits.Details.Services;
using Apex.Invest.Features.Modules.Deposits.List.Mapper;
using Apex.Invest.Features.Modules.Deposits.List.Services;
using Apex.Invest.Features.Modules.Shares.Details.Mapper;
using Apex.Invest.Features.Modules.Shares.Details.Services;
using Apex.Invest.Features.Modules.Shares.List.Mapper;
using Apex.Invest.Features.Modules.Shares.List.Services;
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
            factory.RegisterMapper(() => new ShareDetailsMapper());
            factory.RegisterMapper(() => new ShareListMapper());
            factory.RegisterMapper(() => new DepositDetailsMapper());
            factory.RegisterMapper(() => new DepositListMapper());

            return factory;
        });

        services.AddScoped<BondDetailsService>();
        services.AddScoped<IBondDetailsService>(sp =>
        {
            var service = sp.GetRequiredService<BondDetailsService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new BondDetailsDecorator(service, validator);
        });

        services.AddScoped<ShareDetailsService>();
        services.AddScoped<IShareDetailsService>(sp =>
        {
            var service = sp.GetRequiredService<ShareDetailsService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new ShareDetailsDecorator(service, validator);
        });

        services.AddScoped<DepositDetailsService>();
        services.AddScoped<IDepositDetailsService>(sp =>
        {
            var service = sp.GetRequiredService<DepositDetailsService>();
            var validator = sp.GetRequiredService<IValidationRunner>();
            return new DepositDetailsDecorator(service, validator);
        });

        services.AddScoped<IBondListService, BondListService>();
        services.AddScoped<IShareListService, ShareListService>();
        services.AddScoped<IDepositListService, DepositListService>();
    }
}
