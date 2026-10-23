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

/*  
 *  Use next commands to support new migrations and update the DB
 *  $env:DefaultConnection = 'Host=localhost;Port=5432;Database=Apex.Portfolio;Username=postgres;Password=12345'
 *  dotnet ef migrations add InitBaseTables --project src/Apex.Portfolio.Migrations --startup-project src/Apex.Portfolio.Migrations --context EntitiesDbContext
 *  dotnet ef database update --project src/Apex.Portfolio.Migrations --startup-project src/Apex.Portfolio.Migrations --context EntitiesDbContext
 */