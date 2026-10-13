using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Apex.Portfolio.Migrations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntitiesDbContext>
{
    public EntitiesDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
            ?? throw new InvalidOperationException($@"Environment variable DefaultConnection is not set.");

        var optionsBuilder = new DbContextOptionsBuilder<EntitiesDbContext>();

        optionsBuilder
            .UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.GetName().Name));

        return new EntitiesDbContext(optionsBuilder.Options);
    }
}