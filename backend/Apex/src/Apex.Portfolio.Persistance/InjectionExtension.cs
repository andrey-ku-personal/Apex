using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Persistance;

public static class InjectionExtension
{
    public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<EntitiesDbContext>(
            options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), x =>
                    {
                        x.MigrationsAssembly("Apex.Portfolio.Migrations");
                        x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });

#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            }, ServiceLifetime.Transient);

        services.AddScoped(p => p.GetRequiredService<IDbContextFactory<EntitiesDbContext>>().CreateDbContext());
    }
}
