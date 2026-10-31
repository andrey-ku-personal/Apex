using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Users.Persistance;

public static class InjectionExtension
{
    public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<UsersDbContext>(options => options
            .UseNpgsql(connectionString, m => m.MigrationsAssembly("Apex.Users.Migrations")));
    }
}
