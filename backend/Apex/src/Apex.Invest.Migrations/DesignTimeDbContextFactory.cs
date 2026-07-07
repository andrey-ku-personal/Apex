using Apex.Invest.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Apex.Invest.Migrations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntitiesDbContext>
{
    public EntitiesDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
            ?? throw new InvalidOperationException($@"
Environment variable DefaultConnection is not set.
PowerShell: $env:DefaultConnection = 'Host=localhost;Port=5432;Database=gaia_demeter;Username=postgres;Password=secret'
CMD: set GAIA_DB_CONNECTION=Host=localhost;Port=5432;...");

        var optionsBuilder = new DbContextOptionsBuilder<EntitiesDbContext>();

        optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql(connectionString,
                x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.GetName().Name));

        return new EntitiesDbContext(optionsBuilder.Options);
    }
}