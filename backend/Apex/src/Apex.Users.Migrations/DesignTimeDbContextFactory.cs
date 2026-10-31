using Apex.Users.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Apex.Users.Migrations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                @"Connection string DefaultConnection is not set: add ConnectionStrings:DefaultConnection to appsettings.json.");

        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

        optionsBuilder
            .UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.GetName().Name));

        return new UsersDbContext(optionsBuilder.Options);
    }
}

/*
    Use next commands to support new migrations and update the DB.
    The connection string is read from appsettings.json; the ConnectionStrings__DefaultConnection environment variable overrides it.

    dotnet ef migrations add MigrationName --project src/Apex.Users.Migrations --startup-project src/Apex.Users.Migrations --context UsersDbContext
    dotnet ef database update --project src/Apex.Users.Migrations --startup-project src/Apex.Users.Migrations --context UsersDbContext
 */
