using Apex.Invest.Domain.Entities;
using Apex.Invest.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace Apex.Invest.Features.Tests;

[CollectionDefinition(nameof(SliceFixture))]
public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

public class SliceFixture : IAsyncLifetime
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WebApplicationFactory<Program> _factory;

    public FinancePlatform[] Platforms { get; set; }

    public SliceFixture()
    {
        _factory = new CustomTestApplicationFactory();

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task InitializeAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<EntitiesDbContext>();
        await dbContext.Database.OpenConnectionAsync();

        using var connection = dbContext.Database.GetDbConnection();

        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory", "FinancePlatform"],
            DbAdapter = DbAdapter.Postgres
        });

        await respawner.ResetAsync(connection);

        Platforms = [.. dbContext.Set<FinancePlatform>()];
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EntitiesDbContext>();

        try
        {
            await action(scope.ServiceProvider);
        }
        catch (Exception)
        {
            dbContext.RollbackTransaction();
            throw;
        }
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<EntitiesDbContext>();

        try
        {
            var result = await action(scope.ServiceProvider);

            return result;
        }
        catch (Exception)
        {
            dbContext.RollbackTransaction();
            throw;
        }
    }

    public async Task<TResult> UseServiceAsync<TService, TResult>(Func<TService, Task<TResult>> action)
        where TService : notnull
    {
        return await ExecuteScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<TService>();
            return await action(service);
        });
    }

    public async Task DisposeAsync()
    {
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }
    }
}

public class CustomTestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddJsonFile("appsettings.json");
            configBuilder.AddEnvironmentVariables();
        });
    }
}
