using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Invest.Persistance;

public static class InjectionExtension
{
    public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<EntitiesDbContext>(
            options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), x =>
                    {
                        x.MigrationsAssembly("Apex.Invest.Migrations");
                        x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });

#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            }, ServiceLifetime.Transient);

        services.AddScoped(p => p.GetRequiredService<IDbContextFactory<EntitiesDbContext>>().CreateDbContext());
    }
}
