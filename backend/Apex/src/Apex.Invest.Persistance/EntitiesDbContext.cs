using Apex.Invest.Domain.Abstractions;
using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Apex.Invest.Persistance;

public class EntitiesDbContext(DbContextOptions<EntitiesDbContext> options) : DbContext(options)
{
    public DbSet<FinanceInstrument> FinanceInstruments => Set<FinanceInstrument>();
    public DbSet<Share> Shares => Set<Share>();
    public DbSet<Bond> Bonds => Set<Bond>();
    public DbSet<Deposit> Deposits => Set<Deposit>();
    public DbSet<DepositOperation> DepositOperations => Set<DepositOperation>();
    public DbSet<FinancePlatform> FinancePlatforms => Set<FinancePlatform>();

    protected IDbContextTransaction? _currentTransaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntitiesDbContext).Assembly);
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public void BeginTransaction()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = Database.BeginTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            TrackChanges();
            await SaveChangesAsync().ConfigureAwait(false);
            if (_currentTransaction != null) await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public void CommitTransaction()
    {
        try
        {
            TrackChanges();
            SaveChanges();
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    protected void TrackChanges()
    {
        foreach (var entry in ChangeTracker.Entries<AnalyticalEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
            }
    }
}